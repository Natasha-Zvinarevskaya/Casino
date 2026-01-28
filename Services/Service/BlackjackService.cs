using Azure;
using Casino.DataContext;
using Casino.DataContext.Enums;
using Casino.DataContext.Enums.BlackjackGame;
using Casino.DataContext.Models.BlackjackGame;
using Casino.Services.Interfaces;
using Casino.Services.Models;
using Casino.Services.RequestResponse.BlackjackGame.Requests;
using Casino.Services.RequestResponse.BlackjackGame.Response;
using Casino.Services.RequestResponse.PlayerGameService.Request;
using Casino.Services.RequestResponse.UserService.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace Casino.Services.Service
{
    public class BlackjackService : IBlackJackGameService
    {
        private IPlayerGameService _playerGameService;
        private IUserService _userService;
        private DbContextOptions<CasinoDbContext> _options;
        public BlackjackService(IPlayerGameService playerGameService, IUserService userService, DbContextOptions<CasinoDbContext> options)
        {
            _playerGameService = playerGameService;
            _userService = userService;
            _options = options;
        }

        /// <summary>
        /// Старт игры при нажатии кнопки на сайте 
        /// </summary>
        /// <param name="request">Ид игры и Сумма ставки , выбранная пользователем</param>
        /// <param name="userIds">Список Ид пользователей</param>
        /// <returns></returns>
        public BaseResponse<BlackJackGameModel> Play(BlackjackPlayRequest request)
        {
            /// <summary>
            /// Колода
            /// </summary>
            Deck deck = new Deck();
            /// <summary>
            /// Рука игрока
            /// </summary>
            List<PlayerModel> players = new List<PlayerModel>();
            List<int> userIds = _userService.GetListUsersId(new GetListUsersIdsRequest { GameId = request.GameId }).Data;
            /// <summary>
            /// Рука дилера
            /// </summary>
            PlayerModel dealer = new PlayerModel() { IsDealer = true, Cards = [], StatusGame = EnumStatusPlayerGame.None };
            players.Add(dealer);
            foreach (var userId in userIds)
            {
                players.Add(new PlayerModel { IsDealer = false, UserId = userId, Cards = [], StatusGame = EnumStatusPlayerGame.None });
            }
            List<Card> gameDeck = deck.Shuffle();

            foreach (var player in players)
            {
                player.Cards.Add(deck.DealCard());
                player.Cards.Add(deck.DealCard());
            }

            var saveGameHistoryRequest = new SaveGameHistoryRequest
            {
                Deck = deck.GetCards(),
                PlayersHands = players,
                GameId = request.GameId,
                DealerCheating = false
            };
            _playerGameService.SaveGameHistory(saveGameHistoryRequest);
            var blackjackGameModel = new BlackJackGameModel { GameId = request.GameId, Status = EnumStatusGame.None, PLayerCards = players };
            return new BaseResponse<BlackJackGameModel>(blackjackGameModel);
        }
        /// <summary>
        /// Ход игрока
        /// </summary>
        /// <param name="gameId"></param>
        /// <param name="userId"></param>
        /// <returns></returns> 
        public BaseResponse<BlackJackGameModel> Turn(TurnPlayerRequest request)
        {
            var gameHistory = _playerGameService.GetHistory(new GetHistoryRequest { GameId = request.GameId });
            Deck deck = new Deck();
            deck.HistoryDeck(gameHistory.CardsHistory.Deck);
            List<PlayerModel> players = gameHistory.CardsHistory.Players;

            var index = players.FindIndex(x => x.UserId == request.UserId);
            players[index].Cards.Add(deck.DealCard());
            if (players[index].Score > 21)
            {
                players[index].StatusGame = EnumStatusPlayerGame.WaitingEndGame;
                var waitingPlayers = players.Where(x => x.StatusGame == EnumStatusPlayerGame.WaitingEndGame).Select(x => x.StatusGame).ToList();
                if (waitingPlayers.Count == players.Count - 1)
                {
                    var dealerTurn = DealerTurn(new TurnPlayerRequest { GameId = request.GameId, UserId = null });
                    var requestGameOver = DetermineWinner(new DetermineWinnerRequest { GameId = request.GameId });
                    return new BaseResponse<BlackJackGameModel>(requestGameOver.Data);
                }
            }

            var saveGameHistoryRequest = new SaveGameHistoryRequest { Deck = deck.GetCards(), PlayersHands = players, GameId = request.GameId, DealerCheating = false };
            _playerGameService.SaveGameHistory(saveGameHistoryRequest);
            var blackjackGameModel = new BlackJackGameModel { GameId = request.GameId, Status = EnumStatusGame.None, PLayerCards = players };
            return new BaseResponse<BlackJackGameModel>(blackjackGameModel);
        }

        private BaseResponse<BlackJackGameModel> DealerTurn(TurnPlayerRequest request)
        {
            var gameHistory = _playerGameService.GetHistory(new GetHistoryRequest { GameId = request.GameId });
            Deck deck = new Deck();
            deck.HistoryDeck(gameHistory.CardsHistory.Deck);
            List<PlayerModel> players = gameHistory.CardsHistory.Players;

            var index = players.FindIndex(x => x.UserId == request.UserId);
            var dealerSetting = GetDealerSetting();
            bool risk = false;
            bool cheating = false;
            bool winCheating = false;
            int score = 0;

            //Вероятность ,что дилер рискнет и возьмет дополнительную карту
            Random rnd = new Random();
            float res = rnd.Next(101);
            if (res <= dealerSetting.PercentRisk)
                risk = true;
            //Вероятность ,что дилер сжульничает и возьмет из колоды нужную карту
            res = rnd.Next(101);
            if (res <= dealerSetting.PercentСheating)
                cheating = true;
            //Вероятность ,что дилер возьмет карту для 21 или 20 очков
            res = rnd.Next(101);
            if (res <= dealerSetting.PercentWinCheating)
                winCheating = true;


            //Если у дилера очков больше или равно, чем в его настройках, проверяем можем ли дилер рисковать и брать еще одну карту
            if (players[index].Score >= dealerSetting.CountPoints)
            {
                if (risk)
                    players[index].Cards.Add(deck.DealCard());
            }
            else
                players[index].Cards.Add(deck.DealCard());


            //Если дилеру выпало сжульничать
            if (cheating == true)
            {
                //Если выпало победное жульничество
                if (winCheating == true)
                {
                    score = 21;
                }
                else
                {
                    score = 20;
                }

                var requiredValue = score - players[index].Score;
                Card spareCard = null;
                var cards = deck.GetCards();
                foreach (var card in cards)
                {
                    if (spareCard == null)
                        spareCard = card;
                    if ((int)card.Value == requiredValue)
                    {
                        players[index].Cards.Add(card);
                        break;
                    }
                    if (card.Value == DataContext.Enums.BlackjackGame.CardValue.Ace && requiredValue == 1)
                    {
                        players[index].Cards.Add(card);
                        break;
                    }

                    if ((int)card.Value < requiredValue && card.Value > spareCard.Value)
                        spareCard = card;
                    //Если не хватает до победы 2 очков, тогда запасная карта будет туз 
                    if (card.Value == DataContext.Enums.BlackjackGame.CardValue.Ace && requiredValue == 2)
                        spareCard = card;
                }
                if (players[index].Score < 21)
                    players[index].Cards.Add(spareCard);
            }

            var saveGameHistoryRequest = new SaveGameHistoryRequest { Deck = deck.GetCards(), PlayersHands = players, GameId = request.GameId, DealerCheating = cheating };
            _playerGameService.SaveGameHistory(saveGameHistoryRequest);
            var blackjackGameModel = new BlackJackGameModel { GameId = request.GameId, Status = EnumStatusGame.None, PLayerCards = players };
            return new BaseResponse<BlackJackGameModel>(blackjackGameModel);
        }

        /// <summary>
        /// Игрок заканчивает ход
        /// </summary>
        /// <param name="gameId"> Ид игры</param>
        /// <param name="userId">Юзер Ид</param>
        /// <returns></returns>
        public BaseResponse<BlackJackGameModel> SkipPlayer(SkipPlayerRequest req)
        {
            var gameHistory = _playerGameService.GetHistory(new GetHistoryRequest { GameId = req.GameId });
            Deck deck = new Deck();
            deck.HistoryDeck(gameHistory.CardsHistory.Deck);
            var saveGameHistoryRequest = new SaveGameHistoryRequest()
            {
                Deck = deck.GetCards(),
                PlayersHands = gameHistory.CardsHistory.Players,
                GameId = req.GameId,
                DealerCheating = false
            };

            List<PlayerModel> players = gameHistory.CardsHistory.Players;
            var index = players.FindIndex(x => x.UserId == req.UserId);
            players[index].StatusGame = EnumStatusPlayerGame.WaitingEndGame;
            _playerGameService.SaveGameHistory(saveGameHistoryRequest);


            var waitingPlayers = players.Where(x => x.StatusGame == EnumStatusPlayerGame.WaitingEndGame).Select(x => x.StatusGame).ToList();
            if (waitingPlayers.Count == players.Count - 1)
            {
                var dealerTurn = DealerTurn(new TurnPlayerRequest { GameId = req.GameId, UserId = null });
                var requestGameOver = DetermineWinner(new DetermineWinnerRequest { GameId = req.GameId });
                return new BaseResponse<BlackJackGameModel>(requestGameOver.Data);
            }
            var request = new BlackJackGameModel { GameId = req.GameId, Status = EnumStatusGame.None, PLayerCards = gameHistory.CardsHistory.Players };
            return new BaseResponse<BlackJackGameModel>(request);
        }
        /// <summary>
        /// Определение победителя
        /// </summary>
        /// <param name="gameId">ИД игры</param>
        /// <returns>модель блэкджека с инфо о статусе игры и картах на руках для фронта</returns>
        private BaseResponse<BlackJackGameModel> DetermineWinner(DetermineWinnerRequest request)
        {
            bool dealerWin = false;
            bool dealerDraw = false;
            EnumStatusGame statusGame = EnumStatusGame.None;
            //Нужен только ид игры
            var gameHistory = _playerGameService.GetHistory(new GetHistoryRequest() { GameId = request.GameId });
            var players = gameHistory.CardsHistory.Players;

            //Проверяем есть ли хоть один игрок набравший 21 очко и делим игроков на победителей и проигравших
            var winners = players.Where(x => x.Score == 21).ToList();
            var losers = players.Except(winners).ToList();

            if (winners.Count != 0)
            {
                //Если больше одного игрока набрали 21 очко 
                if (winners.Count() > 1)
                {
                    //Устанавливаем им ничью,а остальным проигрыш
                    foreach (var winner in winners)
                    {
                        winner.StatusGame = EnumStatusPlayerGame.Draw;
                        if (winner.IsDealer)
                            dealerDraw = true;
                    }
                    foreach (var loser in losers)
                    {
                        loser.StatusGame = EnumStatusPlayerGame.Loss;
                    }
                }
                else
                {
                    //Если только 1 игрок набрал 21 очко
                    foreach (var winner in winners)
                    {
                        winner.StatusGame = EnumStatusPlayerGame.Win;
                        if (winner.IsDealer)
                            dealerWin = true;
                    }
                    foreach (var loser in losers)
                    {
                        loser.StatusGame = EnumStatusPlayerGame.Loss;
                    }
                }
            }
            else
            {
                //Если нет ни одного игрока,который набрал 21 очко, то ищем того,кто был ближе всего в этому числу, не превышая его
                //И делим их на тех кто превысил 21 (проигравшие) и тех,кто не превысил (победители)
                winners = players.Where(x => x.Score < 21).ToList();
                losers = players.Except(winners).ToList();

                //Если есть игроки, кто не превысил 21 очко,ищем кто был ближе всего к этому числу
                if (winners.Count != 0)
                {
                    int checkScore = 0;

                    foreach (var winner in winners)
                    {
                        if (winner.Score > checkScore)
                            checkScore = winner.Score;
                    }
                    //Находим всех игроков,кто набрал максимально близкое число к 21 и делим игроков на победителей и проигравших
                    winners = winners.Where(x => x.Score == checkScore).ToList();
                    losers = players.Except(winners).ToList();


                    if (winners.Count != 0)
                    {
                        //Если победителей больше  одного,то записываем им ничью, а остальным проигрыш
                        if (winners.Count() > 1)
                        {

                            foreach (var winner in winners)
                            {
                                winner.StatusGame = EnumStatusPlayerGame.Draw;
                                if (winner.IsDealer)
                                    dealerDraw = true;
                            }
                            foreach (var loser in losers)
                            {
                                loser.StatusGame = EnumStatusPlayerGame.Loss;
                            }
                        }
                        //Если победитель один
                        else
                        {
                            foreach (var winner in winners)
                            {
                                winner.StatusGame = EnumStatusPlayerGame.Win;
                                if (winner.IsDealer)
                                    dealerWin = true;
                            }
                            foreach (var loser in losers)
                            {
                                loser.StatusGame = EnumStatusPlayerGame.Loss;
                            }
                        }
                        //Объединяем 2 списка обратно в players и для каждого игрока проверяем не дилер ли он и заканчиваем игру
                        players = winners.Union(losers).ToList();
                        List<EndGamePlayer> endGamePlayers = new List<EndGamePlayer>();
                        foreach (var player in players)
                        {
                            if (player.UserId != null)
                                endGamePlayers.Add(new EndGamePlayer() { StatusGame = player.StatusGame, UserId = (int)player.UserId });
                        }
                        if (dealerDraw)
                        {
                            _playerGameService.EndGame(new EndGameRequest { GameId = request.GameId, ResultGame = EnumStatusGame.Draw, Players = endGamePlayers });
                            statusGame = EnumStatusGame.Draw;
                        }
                        else
                            if (dealerWin)
                        {
                            _playerGameService.EndGame(new EndGameRequest { GameId = request.GameId, ResultGame = EnumStatusGame.DealerWin, Players = endGamePlayers });
                            statusGame = EnumStatusGame.DealerWin;
                        }
                        else
                        {
                            _playerGameService.EndGame(new EndGameRequest { GameId = request.GameId, ResultGame = EnumStatusGame.DealerLoss, Players = endGamePlayers });
                            statusGame = EnumStatusGame.DealerLoss;
                        }
                    }
                }
                //Если все игроки набрали больше 21 очка, тогда им всем записываем проигрыш
                else
                {
                    List<EndGamePlayer> endGamePlayers = new List<EndGamePlayer>();
                    foreach (var player in players)
                    {
                        player.StatusGame = EnumStatusPlayerGame.Loss;
                        if (player.UserId != null)
                            endGamePlayers.Add(new EndGamePlayer() { StatusGame = player.StatusGame, UserId = (int)player.UserId });
                    }
                    statusGame = EnumStatusGame.Draw;

                    _playerGameService.EndGame(new EndGameRequest { GameId = request.GameId, ResultGame = EnumStatusGame.Draw, Players = endGamePlayers });
                }
            }
            var blackjackGameModel = new BlackJackGameModel { GameId = request.GameId, Status = statusGame, PLayerCards = players };
            return new BaseResponse<BlackJackGameModel>(blackjackGameModel);
        }
        /// <summary>
        /// Получить настройки дилера
        /// </summary>
        /// <returns></returns>
        private GetDealerSettingResponse GetDealerSetting()
        {
            var db = new CasinoDbContext(_options);
            var queryDealerSetting = db.DealerBJSettings;

            var res = queryDealerSetting.Select(x => new GetDealerSettingRequest
            {
                Id = x.Id,
                Settings = x.Settings

            }).ToList();

            GetDealerSettingResponse response = new GetDealerSettingResponse ();
           foreach (var item in res)
            {
                if (item.Id == EnumBlackJackGameSettings.PercentRisk)
                    response.PercentRisk = float.Parse(item.Settings);

                if (item.Id == EnumBlackJackGameSettings.CountPoints)
                    response.CountPoints = int.Parse(item.Settings);

                if (item.Id == EnumBlackJackGameSettings.PercentWinCheating)
                    response.PercentWinCheating = float.Parse(item.Settings);

                if (item.Id == EnumBlackJackGameSettings.PercentСheating)
                    response.PercentСheating = float.Parse(item.Settings);
            }
            return response;
            //var response = new GetDealerSettingResponse
            //{
            //    CountPoints = queryDealerSetting.CountPoints,
            //    PercentRisk = queryDealerSetting.PercentRisk,
            //    PercentWinCheating = queryDealerSetting.PercentWinCheating,
            //    PercentСheating = queryDealerSetting.PercentСheating
            //};

        }

    }
}