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

namespace Casino.Services.Service
{
    public class BlackjackService:IBlackJackGameService
    {
        private IPlayerGameService _playerGameService;
        public BlackjackService(IPlayerGameService playerGameService)
        {
            _playerGameService = playerGameService;

        }
        /// <summary>
        /// Колода
        /// </summary>
        private Deck _deck = new Deck();
        /// <summary>
        /// Рука игрока
        /// </summary>
        private List<Card> _playerHand = new List<Card>();
        /// <summary>
        /// Рука дилера
        /// </summary>
        private List<Card> _dealerHand = new List<Card>();
        /// <summary>
        /// Счет
        /// </summary>
        private int _playerScore = 0;
        private int _dealerScore = 0;
        /// <summary>
        /// Старт игры при нажатии кнопки на сайте 
        /// </summary>
        /// <param name="request">Сумма ставки , выбранная пользователем</param>
        /// <param name="userId">пользователь</param>
        /// <returns></returns>
        public BaseResponse<BlackJackGameModel> Play(BlackjackPlayRequest request,int userId)
        {
            var startRequest = new StartGameRequest
            {
                Bet = request.Bet,
                UserId = userId,
                Game = 1
            };
            var gameId=_playerGameService.StartGame(startRequest);
            List<Card> gameDeck = _deck.Shuffle();
            _playerHand.Add(_deck.DealCard());
            _playerHand.Add(_deck.DealCard());

            _dealerHand.Add(_deck.DealCard());
            _dealerHand.Add(_deck.DealCard());

            var cardsDeck = _deck.GetCards();
            var saveGameHistoryRequest = new SaveGameHistoryRequest { Deck = cardsDeck, PlayerHand = _playerHand, DealerHand = _dealerHand, GameId = gameId };
            _playerGameService.SaveGameHistory(saveGameHistoryRequest);
                var blackjackGameModel = new BlackJackGameModel { GameId = gameId, Status = EnumStatusGame.None, PLayerCards = _playerHand, DealerCards = _dealerHand };
                return new BaseResponse<BlackJackGameModel>(blackjackGameModel);
            //Сделать проверку на выигрыш. (10+11=21). Отправить измененный статус игры (победа/проигрыш)
            
        }
        public BaseResponse<BlackJackGameModel> PlayerTurn(int gameId)
        {
            _playerHand.Add(_deck.DealCard());
            return new BaseResponse<BlackJackGameModel>("");
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
