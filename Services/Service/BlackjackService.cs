using Azure;
using Casino.DataContext;
using Casino.DataContext.Enums;
using Casino.Services.Enums.BlackjackGame;
using Casino.Services.Interfaces;
using Casino.Services.Models;
using Casino.Services.Models.BlackjackGame;
using Casino.Services.RequestResponse.BlackjackGame.Requests;
using Casino.Services.RequestResponse.BlackjackGame.Response;
using Casino.Services.RequestResponse.PlayerGameService.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Services.Service
{
    public class BlackjackService : IBlackJackGameService
    {
        private IPlayerGameService _playerGameService;
        public BlackjackService(IPlayerGameService playerGameService)
        {
            _playerGameService = playerGameService;

        }

        /// <summary>
        /// Старт игры при нажатии кнопки на сайте 
        /// </summary>
        /// <param name="request">Ид игры и Сумма ставки , выбранная пользователем</param>
        /// <param name="userIds">Список Ид пользователей</param>
        /// <returns></returns>
        public BaseResponse<BlackJackGameModel> Play(BlackjackPlayRequest request )
        {
            /// <summary>
            /// Колода
            /// </summary>
            //Абстракция
            Deck deck = new Deck();
            /// <summary>
            /// Рука игрока
            /// </summary>
            List<PlayerModel> players = new List<PlayerModel>();
            /// <summary>
            /// Рука дилера
            /// </summary>
            PlayerModel dealer = new PlayerModel() { IsDealer = true, Cards = [], StatusGame = EnumStatusGame.None };
            players.Add(dealer);
            foreach (var userId in request.UserIds)
            {
                players.Add(new PlayerModel { IsDealer = false, UserId = userId, Cards = [], StatusGame = EnumStatusGame.None });
            }

            //var startRequest = new StartGameRequest
            //{
            //    Bet = request.Bet,
            //    UserIds = request.UserIds,
            //    Game = 1
            //};
            //var gameId = _playerGameService.StartGame(startRequest);
            List<Card> gameDeck = deck.Shuffle();

            foreach (var player in players)
            {
                player.Cards.Add(deck.DealCard());
                player.Cards.Add(deck.DealCard());
            }

            //dealerHand.Cards.Add(deck.DealCard());
            //dealerHand.Cards.Add(deck.DealCard());
            //_playersScores = GetHandScore(_playersHands);
            //_dealerScore = GetHandScore(_dealerHand);
            ////
            //if (_playerScore == 21 && _dealerScore == 21)
            //{
            //    var endGameRequest = new EndGameRequest() { GameId = gameId };
            //    endGameRequest.ResultGame = EnumStatusGame.Draw;
            //    _playerGameService.EndGame(endGameRequest);
            //    var blackjackGameModel = new BlackJackGameModel { GameId = gameId, Status = EnumStatusGame.Draw, PLayerCards = _playerHand, DealerCards = _dealerHand };
            //    return new BaseResponse<BlackJackGameModel>(blackjackGameModel);
            //}
            //else
            //if (_playerScore == 21)
            //{
            //    var endGameRequest = new EndGameRequest() { GameId = gameId };
            //    endGameRequest.ResultGame = EnumStatusGame.Win;
            //    _playerGameService.EndGame(endGameRequest);
            //    var blackjackGameModel = new BlackJackGameModel { GameId = gameId, Status = EnumStatusGame.Win, PLayerCards = _playerHand, DealerCards = _dealerHand };
            //    return new BaseResponse<BlackJackGameModel>(blackjackGameModel);
            //}
            //else
            //    if (_dealerScore == 21)
            //{
            //    var endGameRequest = new EndGameRequest() { GameId = gameId };
            //    endGameRequest.ResultGame = EnumStatusGame.Loss;
            //    _playerGameService.EndGame(endGameRequest);
            //    var blackjackGameModel = new BlackJackGameModel { GameId = gameId, Status = EnumStatusGame.Loss, PLayerCards = _playerHand, DealerCards = _dealerHand };
            //    return new BaseResponse<BlackJackGameModel>(blackjackGameModel);
            //}
            //else
            //{
            //  var cardsDeck = _deck.GetCards();
            var saveGameHistoryRequest = new SaveGameHistoryRequest
            {
                Deck = deck.GetCards(),
                PlayersHands = players,
                GameId = request.GameId
            };
            _playerGameService.SaveGameHistory(saveGameHistoryRequest);
            var blackjackGameModel = new BlackJackGameModel { GameId = request.GameId, Status = EnumStatusGame.None, PLayerCards = players };
            return new BaseResponse<BlackJackGameModel>(blackjackGameModel);
        }


        //}
        ///// <summary>
        ///// Сумма очков каждого игрока
        ///// </summary>
        ///// <param name="cards">Карты в руке</param>
        ///// <returns>Счет</returns>
        //private List<int> GetHandScore(List<PlayerModel> playersHands)
        //{
        //    List<int> scores = [];
        //    int aces = 0;
        //    foreach (var player in playersHands)
        //    {
        //        int score = 0;
        //        foreach (var card in player.Cards)
        //        {
        //            score = score + (int)card.Value;
        //            if (card.Value == CardValue.Ace)
        //                aces++;
        //        }

        //        while (score > 21 && aces > 0)
        //        {
        //            score -= 10;
        //            aces--;
        //        }
        //        scores.Add(score);

        //    }

        //    return scores;
        //}


        /// <summary>
        /// Ход игрока
        /// </summary>
        /// <param name="gameId"></param>
        /// <param name="userId"></param>
        /// <returns></returns> 
        //public BaseResponse<BlackJackGameModel> Turn(int gameId, int userId)
        //{
        //    List<int> _playersScores = [];
        //    var gameHistory = _playerGameService.GetHistory(new GetHistoryRequest { GameId = gameId });
        //    /// <summary>
        //    /// Колода
        //    /// </summary>
        //    Deck _deck = new Deck();
        //    _deck.HistoryDeck(gameHistory.CardsHistory.Deck);
        //    /// <summary>
        //    /// Рука игрока
        //    /// </summary>
        //    List<PlayerModel> _playersHand = gameHistory.CardsHistory.PlayersHand;
        //    List<Card> _dealerHand = gameHistory.CardsHistory.DealerHand;

        //    foreach (var player in _playersHand)
        //    {
        //        player.Add(_deck.DealCard());

        //        _playersScores.Add(GetHandScore(player));


        //    }

        //    var cardsDeck = _deck.GetCards();
        //    var saveGameHistoryRequest = new SaveGameHistoryRequest { Deck = cardsDeck, PlayersHands = _playersHand, DealerHand = _dealerHand, GameId = gameId };
        //    _playerGameService.SaveGameHistory(saveGameHistoryRequest);
        //    var blackjackGameModel = new BlackJackGameModel { GameId = gameId, Status = EnumStatusGame.None, PLayerCards = _playersHand, DealerCards = _dealerHand };
        //    return new BaseResponse<BlackJackGameModel>(blackjackGameModel);
        //}

        /// <summary>
        /// Ход игрока
        /// </summary>
        /// <param name="gameId"></param>
        /// <param name="userId"></param>
        /// <returns></returns> 
        public BaseResponse<BlackJackGameModel> Turn(TurnPlayerRequest request) 
        {
            var gameHistory = _playerGameService.GetHistory(new GetHistoryRequest { GameId =request.GameId});
            Deck deck = new Deck();
            deck.HistoryDeck(gameHistory.CardsHistory.Deck);
            List<PlayerModel> players = gameHistory.CardsHistory.Players;

            players.FirstOrDefault(x => x.UserId == request.UserId).Cards.Add(deck.DealCard()); 
            

            var saveGameHistoryRequest = new SaveGameHistoryRequest { Deck = deck.GetCards(), PlayersHands = players, GameId = request.GameId };
            _playerGameService.SaveGameHistory(saveGameHistoryRequest);
            var blackjackGameModel = new BlackJackGameModel { GameId = request.GameId, Status = EnumStatusGame.None, PLayerCards = players };
            return new BaseResponse<BlackJackGameModel>(blackjackGameModel);
        }
        ///// <summary>
        ///// Игрок заканчивает ход
        ///// </summary>
        ///// <param name="gameId"> Ид игры</param>
        ///// <param name="userId">Юзер Ид</param>
        ///// <returns></returns>
        //public BaseResponse<BlackJackGameModel> SkipPlayer(int gameId, int userId)
        //{
        //    List<int> _playerScore;
        //    var gameHistory = _playerGameService.GetHistory(new GetHistoryRequest { GameId = gameId });
        //    Deck _deck = new Deck();
        //    _deck.HistoryDeck(gameHistory.CardsHistory.Deck);
        //    List<PlayerModel> _playersHands = gameHistory.CardsHistory.Players;
        //    _playersScore = GetHandScore(_playersHands);
        //    var dealerTurn = Turn(new DealerTurnRequest { DealerHand = gameHistory.CardsHistory.DealerHand, Deck = _deck });
        //    var saveGameHistoryRequest = new SaveGameHistoryRequest { Deck = dealerTurn.Data.Deck, PlayerHand = _playerHand, DealerHand = dealerTurn.Data.DealerHand, GameId = gameId };
        //    _playerGameService.SaveGameHistory(saveGameHistoryRequest);
        //    var request = new DetermineWinnerRequest { GameId = gameId, UserId = userId, PlayerScore = _playerScore, DealerScore = dealerTurn.Data.DealerScore };

        //    var blackJackGameModel = DetermineWinner(request);
        //    return new BaseResponse<BlackJackGameModel>(blackJackGameModel.Data);
        //}

        /// <summary>
        /// Игрок заканчивает ход
        /// </summary>
        /// <param name="gameId"> Ид игры</param>
        /// <param name="userId">Юзер Ид</param>
        /// <returns></returns>
        public BaseResponse<BlackJackGameModel> SkipPlayer(BaseGameIdReq req)
        {
            var gameHistory = _playerGameService.GetHistory(new GetHistoryRequest { GameId = req.GameId });
            Deck deck = new Deck();
            deck.HistoryDeck(gameHistory.CardsHistory.Deck);
            var saveGameHistoryRequest = new SaveGameHistoryRequest()
            {
                Deck = deck.GetCards(),
                PlayersHands = gameHistory.CardsHistory.Players,
                GameId = req.GameId,
                PlayersSkiped = gameHistory.PlayersSkiped + 1
            };
            _playerGameService.SaveGameHistory(saveGameHistoryRequest);

            if (saveGameHistoryRequest.PlayersSkiped == gameHistory.CardsHistory.Players.Count)
            {
                var dealerTurn = Turn(new TurnPlayerRequest { GameId = req.GameId, UserId = null });
                var requestGameOver = DetermineWinner( new DetermineWinnerRequest { GameId = req.GameId });
                return new BaseResponse<BlackJackGameModel>(requestGameOver.Data);
            }

            var request = new BlackJackGameModel { GameId = req.GameId, Status = EnumStatusGame.None, PLayerCards = gameHistory.CardsHistory.Players};
            return new BaseResponse<BlackJackGameModel>(request);
        }

        ///// <summary>
        ///// Ход дилера
        ///// </summary>
        ///// <param name="request">Карты в руке дилера и карты в колоде  </param>
        ///// <returns></returns>
        //private BaseResponse<DealerTurnResponse> Turn(DealerTurnRequest request)
        //{

        //    while (GetHandScore(request.DealerHand) < 17)
        //    {
        //        request.DealerHand.Cards.Add(request.Deck.DealCard());
        //    }
        //    var response = new DealerTurnResponse
        //    {
        //        DealerHand = request.DealerHand,
        //        DealerScore = GetHandScore(request.DealerHand),
        //        Deck = request.Deck.GetCards()
        //    };
        //    return new BaseResponse<DealerTurnResponse>(response);
        //}

        ///// <summary>
        ///// Определение победителя
        ///// </summary>
        ///// <param name="gameId">ИД игры</param>
        ///// <returns>модель блэкджека с инфо о статусе игры и картах на руках для фронта</returns>
        //private BaseResponse<BlackJackGameModel> DetermineWinner(DetermineWinnerRequest request)
        //{
        //    var gameHistory = _playerGameService.GetHistory(request.GameId, request.UserId);
        //    var _playerHand = gameHistory.CardsHistory.PlayerHand;
        //    var _dealerHand = gameHistory.CardsHistory.DealerHand;

        //    var response = new EndGameRequest() { GameId = request.GameId };
        //    if (request.PlayerScore == request.DealerScore || request.DealerScore > 21 && request.PlayerScore > 21)
        //    {

        //        // Ничья;
        //        response.ResultGame = EnumStatusGame.Draw;
        //        _playerGameService.EndGame(response);
        //        var blackjackGameModel = new BlackJackGameModel { GameId = request.GameId, Status = EnumStatusGame.Draw, PLayerCards = _playerHand, DealerCards = _dealerHand };
        //        return new BaseResponse<BlackJackGameModel>(blackjackGameModel);

        //    }
        //    else
        //    if (request.DealerScore > request.PlayerScore && request.DealerScore <= 21 || request.PlayerScore > 21)
        //    {
        //        // Дилер победил;
        //        response.ResultGame = EnumStatusGame.Loss;
        //        _playerGameService.EndGame(response);
        //        var blackjackGameModel = new BlackJackGameModel { GameId = request.GameId, Status = EnumStatusGame.Loss, PLayerCards = _playerHand, DealerCards = _dealerHand };
        //        return new BaseResponse<BlackJackGameModel>(blackjackGameModel);
        //    }
        //    else

        //    {
        //        // Игрок победил
        //        response.ResultGame = EnumStatusGame.Win;
        //        _playerGameService.EndGame(response);
        //        var blackjackGameModel = new BlackJackGameModel { GameId = request.GameId, Status = EnumStatusGame.Win, PLayerCards = _playerHand, DealerCards = _dealerHand };
        //        return new BaseResponse<BlackJackGameModel>(blackjackGameModel);
        //    }

        //}
        /// <summary>
        /// Определение победителя
        /// </summary>
        /// <param name="gameId">ИД игры</param>
        /// <returns>модель блэкджека с инфо о статусе игры и картах на руках для фронта</returns>
        private BaseResponse<BlackJackGameModel> DetermineWinner(DetermineWinnerRequest request)
        {
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
                        winner.StatusGame = EnumStatusGame.Draw;

                    }
                    foreach (var loser in losers)
                    {
                        loser.StatusGame = EnumStatusGame.Loss;
                    }
                }
                else
                {
                    //Если только 1 игрок набрал 21 очко
                    foreach (var winner in winners)
                    {
                        winner.StatusGame = EnumStatusGame.Win;
                    }
                    foreach (var loser in losers)
                    {
                        loser.StatusGame = EnumStatusGame.Loss;
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
                                winner.StatusGame = EnumStatusGame.Draw;
                            }
                            foreach (var loser in losers)
                            {
                                loser.StatusGame = EnumStatusGame.Loss;
                            }
                        }
                        //Если победитель один
                        else
                        {
                            foreach (var winner in winners)
                            {
                                winner.StatusGame = EnumStatusGame.Win;
                            }
                            foreach (var loser in losers)
                            {
                                loser.StatusGame = EnumStatusGame.Loss;
                            }
                        }
                        //Объединяем 2 списка обратно в players и для каждого игрока проверяем не дилер ли он и заканчиваем игру
                        players = winners.Union(losers).ToList();

                        foreach (var player in players)
                        {
                            if (player.UserId != null)
                                _playerGameService.EndGame(new EndGameRequest { GameId = request.GameId, ResultGame = player.StatusGame, UserId = (int)player.UserId });
                        }
                    }
                }
                //Если все игроки набрали больше 21 очка, тогда им всем записываем проигрыш
                else
                {
                    foreach (var player in players)
                    {
                        player.StatusGame = EnumStatusGame.Loss;
                        if (player.UserId != null)
                            _playerGameService.EndGame(new EndGameRequest { GameId = request.GameId, ResultGame = player.StatusGame, UserId = (int)player.UserId });

                    }
                }
            }
            var blackjackGameModel = new BlackJackGameModel { GameId = request.GameId, Status = EnumStatusGame.GameOver, PLayerCards = players };
            return new BaseResponse<BlackJackGameModel>(blackjackGameModel);
        }
    }
}