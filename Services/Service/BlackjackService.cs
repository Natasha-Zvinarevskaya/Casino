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
        public BaseResponse<BlackJackGameModel> Play(BlackjackPlayRequest request)
        {
            List<Card> gameDeck = _deck.Shuffle();
            _playerHand.Add(_deck.DealCard());
            _playerHand.Add(_deck.DealCard());
           // ShowCardsPlayers();
            _dealerHand.Add(_deck.DealCard());
            _dealerHand.Add(_deck.DealCard());
          
                var blackjackGameModel = new BlackJackGameModel { GameId = 1, Status = EnumStatusGame.None, PlayerCards = _playerHand, DealerCards = _dealerHand };
                return new BaseResponse<BlackJackGameModel>(blackjackGameModel);
                
            
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
