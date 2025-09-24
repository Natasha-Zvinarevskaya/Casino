using Casino.DataContext.Enums;
using Casino.Services.Interfaces;
using Casino.Services.Models.BlackjackGame.Requests;
using Casino.Services.Models.BlackjackGame;
using Casino.Services.Models.PlayerGameService.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Casino.Services.Models.BlackjackGame.Response;
using Casino.Services.Enums.BlackjackGame;
using Casino.DataContext;

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
        /// <param name="request">Сумма ставки , выбранная пользователем</param>
        /// <param name="userId">пользователь</param>
        /// <returns></returns>
        public BaseResponse<BlackJackGameModel> Play(BlackjackPlayRequest request, int userId)
        {
            /// <summary>
            /// Колода
            /// </summary>
            Deck _deck = new Deck();
            /// <summary>
            /// Рука игрока
            /// </summary>
            List<Card> _playerHand = new List<Card>();
            /// <summary>
            /// Рука дилера
            /// </summary>
            List<Card> _dealerHand = new List<Card>();
            /// <summary>
            /// Счет
            /// </summary>
            int _playerScore = 0;
            int _dealerScore = 0;

            var startRequest = new StartGameRequest
            {
                Bet = request.Bet,
                UserId = userId,
                Game = 1
            };
            var gameId = _playerGameService.StartGame(startRequest);
            List<Card> gameDeck = _deck.Shuffle();

            _playerHand.Add(_deck.DealCard());
            _playerHand.Add(_deck.DealCard());

            _dealerHand.Add(_deck.DealCard());
            _dealerHand.Add(_deck.DealCard());
            _playerScore = GetHandScore(_playerHand);
            _dealerScore = GetHandScore(_dealerHand);
            if (_playerScore == 21 && _dealerScore == 21)
            {
                var endGameRequest = new EndGameRequest() { GameId = gameId };
                endGameRequest.ResultGame = EnumStatusGame.Draw;
                _playerGameService.EndGame(endGameRequest);
                var blackjackGameModel = new BlackJackGameModel { GameId = gameId, Status = EnumStatusGame.Draw, PLayerCards = _playerHand, DealerCards = _dealerHand };
                return new BaseResponse<BlackJackGameModel>(blackjackGameModel);
            }
            else
            if (_playerScore == 21)
            {
                var endGameRequest = new EndGameRequest() { GameId = gameId };
                endGameRequest.ResultGame = EnumStatusGame.Win;
                _playerGameService.EndGame(endGameRequest);
                var blackjackGameModel = new BlackJackGameModel { GameId = gameId, Status = EnumStatusGame.Win, PLayerCards = _playerHand, DealerCards = _dealerHand };
                return new BaseResponse<BlackJackGameModel>(blackjackGameModel);
            }
            else
                if (_dealerScore == 21)
            {
                var endGameRequest = new EndGameRequest() { GameId = gameId };
                endGameRequest.ResultGame = EnumStatusGame.Loss;
                _playerGameService.EndGame(endGameRequest);
                var blackjackGameModel = new BlackJackGameModel { GameId = gameId, Status = EnumStatusGame.Loss, PLayerCards = _playerHand, DealerCards = _dealerHand };
                return new BaseResponse<BlackJackGameModel>(blackjackGameModel);
            }
            else
            {
                var cardsDeck = _deck.GetCards();
                var saveGameHistoryRequest = new SaveGameHistoryRequest { Deck = cardsDeck, PlayerHand = _playerHand, DealerHand = _dealerHand, GameId = gameId };
                _playerGameService.SaveGameHistory(saveGameHistoryRequest);
                var blackjackGameModel = new BlackJackGameModel { GameId = gameId, Status = EnumStatusGame.None, PLayerCards = _playerHand, DealerCards = _dealerHand };
                return new BaseResponse<BlackJackGameModel>(blackjackGameModel);
            }
            //Сделать проверку на выигрыш. (10+11=21). Отправить измененный статус игры (победа/проигрыш)

        }
        /// <summary>
        /// Сумма очков у заданной руки 
        /// </summary>
        /// <param name="cards">Карты в руке</param>
        /// <returns>Счет</returns>
        private int GetHandScore(List<Card> cards)
        {
            int score = 0;
            int aces = 0;
            foreach (var card in cards)
            {
                score = score + (int)card.Value;
                if (card.Value == CardValue.Ace)
                    aces++;
            }

            while (score > 21 && aces > 0)
            {
                score -= 10;
                aces--;
            }
            return score;
        }
        public BaseResponse<BlackJackGameModel> PlayerTurn(int gameId, int userId)
        {
            int _playerScore;
            var gameHistory = _playerGameService.GetHistory(gameId, userId);
            /// <summary>
            /// Колода
            /// </summary>
            Deck _deck = new Deck();
             _deck.HistoryDeck(gameHistory.CardsHistory.Deck);
            /// <summary>
            /// Рука игрока
            /// </summary>
            List<Card> _playerHand = gameHistory.CardsHistory.PlayerHand;
            List<Card> _dealerHand = gameHistory.CardsHistory.DealerHand;
            

            _playerHand.Add(_deck.DealCard());
            _playerScore = GetHandScore(_playerHand);


            //if (_playerScore == 21)
            //{

            //    var blackjackGameModel = new BlackJackGameModel { GameId = gameId, Status = EnumStatusGame.Win, PLayerCards = _playerHand, DealerCards = _dealerHand };
            //    return new BaseResponse<BlackJackGameModel>(blackjackGameModel);
            //}
            //else
            //    if (_playerScore > 21)
            //{
            //    var blackjackGameModel = new BlackJackGameModel { GameId = gameId, Status = EnumStatusGame.Loss, PLayerCards = _playerHand, DealerCards = _dealerHand };
            //    return new BaseResponse<BlackJackGameModel>(blackjackGameModel);
            //}
            //else
            //{


            var cardsDeck = _deck.GetCards();
            var saveGameHistoryRequest = new SaveGameHistoryRequest { Deck = cardsDeck, PlayerHand = _playerHand, DealerHand = _dealerHand, GameId = gameId };
            _playerGameService.SaveGameHistory(saveGameHistoryRequest);
            var blackjackGameModel = new BlackJackGameModel { GameId = gameId, Status = EnumStatusGame.None, PLayerCards = _playerHand, DealerCards = _dealerHand };
            return new BaseResponse<BlackJackGameModel>(blackjackGameModel);
            // }
        }
        public BaseResponse<BlackJackGameModel> SkipPlayer(int gameId,int userId)
        {
            int _playerScore;
            int _dealerScore; 
            var gameHistory = _playerGameService.GetHistory(gameId, userId);
            Deck _deck = new Deck();
            _deck.HistoryDeck(gameHistory.CardsHistory.Deck);
            List<Card> _playerHand = gameHistory.CardsHistory.PlayerHand;
            List<Card> _dealerHand = gameHistory.CardsHistory.DealerHand;
            while (GetHandScore(_dealerHand) < 17)
            {
                _dealerHand.Add(_deck.DealCard());
            }
            _playerScore = GetHandScore(_playerHand);
            _dealerScore = GetHandScore(_dealerHand);
            var cardsDeck = _deck.GetCards();
            var saveGameHistoryRequest = new SaveGameHistoryRequest { Deck = cardsDeck, PlayerHand = _playerHand, DealerHand = _dealerHand, GameId = gameId };
            _playerGameService.SaveGameHistory(saveGameHistoryRequest);
            var request = new DetermineWinnerRequest { GameId = gameId, UserId = userId, PlayerScore = _playerScore, DealerScore = _dealerScore };

            var blackJackGameModel = DetermineWinner(request);
            return new BaseResponse<BlackJackGameModel>(blackJackGameModel.Data);
        }
        /// <summary>
        /// Определение победителя
        /// </summary>
        /// <param name="gameId">ИД игры</param>
        /// <returns>модель блэкджека с инфо о статусе игры и картах на руках для фронта</returns>
        private BaseResponse<BlackJackGameModel> DetermineWinner(DetermineWinnerRequest request)
        {
            var gameHistory = _playerGameService.GetHistory(request.GameId, request.UserId);
            var _playerHand = gameHistory.CardsHistory.PlayerHand;
            var _dealerHand = gameHistory.CardsHistory.DealerHand;

            var response = new EndGameRequest() { GameId = request.GameId};
            if (request.PlayerScore == request.DealerScore || request.DealerScore > 21 && request.PlayerScore > 21)
            {

                // Console.WriteLine("Ничья!");
                response.ResultGame = EnumStatusGame.Draw;
                _playerGameService.EndGame(response);
                var blackjackGameModel = new BlackJackGameModel { GameId = request.GameId, Status = EnumStatusGame.Draw, PLayerCards = _playerHand, DealerCards = _dealerHand };
                return new BaseResponse<BlackJackGameModel>(blackjackGameModel);

            }
            else
            if (request.DealerScore > request.PlayerScore && request.DealerScore <= 21 || request.PlayerScore> 21)
            {
                // Console.WriteLine("Дилер победил!");
                    response.ResultGame = EnumStatusGame.Loss;
                _playerGameService.EndGame(response);
                var blackjackGameModel = new BlackJackGameModel { GameId = request.GameId, Status = EnumStatusGame.Loss, PLayerCards = _playerHand, DealerCards = _dealerHand };
                return new BaseResponse<BlackJackGameModel>(blackjackGameModel);
            }
            else

            {
                // Console.WriteLine("Игрок победил!");
                response.ResultGame = EnumStatusGame.Win;
                _playerGameService.EndGame(response);
                var blackjackGameModel = new BlackJackGameModel { GameId = request.GameId, Status = EnumStatusGame.Win, PLayerCards = _playerHand, DealerCards = _dealerHand };
                return new BaseResponse<BlackJackGameModel>(blackjackGameModel);
            }



        }

        //private BaseResponse<BlackJackGameModel> PlayerTurn()
        //{
        //    _playerScore = GetHandScore(_playerHand);
        //    while (_playerScore < 21)
        //    {
        //        ShowScores();
        //        Console.WriteLine("Введите (Н)it - взять карту или (S)tand - остановиться");
        //        var answer = Console.ReadLine().ToUpper();
        //        if (answer == "H")
        //        {
        //            _playerHand.Add(_deck.DealCard());
        //            ShowCardsPlayers();

        //            _playerScore = GetHandScore(_playerHand);
        //            ShowScores();
        //            //Console.WriteLine($"У вас {_playerScore} очков."); 


        //        }
        //        else
        //            break;
        //    }

        //}
    }
}
