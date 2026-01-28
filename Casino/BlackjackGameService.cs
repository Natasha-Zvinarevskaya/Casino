using Casino.DataContext.Enums;
using Casino.DataContext.Enums.BlackjackGame;
using Casino.DataContext.Models.BlackjackGame;
using Casino.Services.Interfaces;
using Casino.Services.RequestResponse.BlackjackGame.Requests;
using Casino.Services.RequestResponse.PlayerGameService.Request;

namespace Casino
{
    /// <summary>
    /// Класс процесса игры
    /// </summary>
    public class BlackjackGameService  
    {
        private IPlayerGameService _playerGameService;
        public BlackjackGameService(IPlayerGameService playerGameService)
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
        private int _playerScore =0;
        private int _dealerScore = 0 ;

        /// <summary>
        /// Метод процесса игры
        /// </summary>
        public void Play(BlackjackPlayRequest request)
        {
                List<Card> gameDeck = _deck.Shuffle();
            _playerHand.Add(_deck.DealCard());
            _playerHand.Add(_deck.DealCard());
            ShowCardsPlayers();
            _dealerHand.Add(_deck.DealCard());
            _dealerHand.Add(_deck.DealCard());
            PlayerTurn();
            if (_playerScore > 21)
            {
                Console.WriteLine("Вы проиграли!");
                var endGameRequest = new EndGameRequest() { ResultGame = EnumStatusGame.DealerWin, GameId = 1 }; //request.GameId};
                _playerGameService.EndGame(endGameRequest);
                ShowScores();
            }

            else
                DealerTurn();

            DetermineWinner(1);//request.GameId);
        }
        /// <summary>
        /// Показать карты в руке игрока
        /// </summary>
        /// <param name="cards"> Карты игрока</param>
        private void ShowCardsPlayers()
        {
            Console.Clear();
            Console.WriteLine("У вас в руке: ");
            foreach (var card in _playerHand)
            {
                card.ToString(card);
            }
        }
        /// <summary>
        /// Показать карты в руке дилера
        /// </summary>
        /// <param name="cards"> Карты дилера </param>
        private void ShowCardsDealer()
        {
            Console.Clear();
            Console.WriteLine("У дилера в руке: ");
            foreach (var card in _dealerHand)
            {
                card.ToString(card);
            }
            // Console.WriteLine($"У дилера {_dealerScore} очков.");
        }


        /// <summary>
        /// Метод хода игрока
        /// </summary>
        private void PlayerTurn()
        {
            _playerScore = GetHandScore(_playerHand);
            while (_playerScore < 21)
            {
                ShowScores();
                Console.WriteLine("Введите (Н)it - взять карту или (S)tand - остановиться");
                var answer = Console.ReadLine().ToUpper();
                if (answer == "H")
                {
                    _playerHand.Add(_deck.DealCard());
                    ShowCardsPlayers();

                    _playerScore = GetHandScore(_playerHand);
                    ShowScores();
                    //Console.WriteLine($"У вас {_playerScore} очков."); 


                }
                else
                    break;
            }

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
        /// <summary>
        /// Ход дилера
        /// </summary>
        private void DealerTurn()
        {
            ShowCardsDealer();
            while (GetHandScore(_dealerHand) < 17)
            {
                _dealerHand.Add(_deck.DealCard());
                ShowCardsDealer();
                _dealerScore = GetHandScore(_dealerHand);
            }
        }
        /// <summary>
        /// Определение победителя
        /// </summary>
        private void DetermineWinner(int gameId)
        {
            var request = new EndGameRequest() { GameId = gameId };
            if (_playerScore == _dealerScore || _dealerScore>21 && _playerScore>21)
            {
                
                Console.WriteLine("Ничья!");
                request.ResultGame = EnumStatusGame.Draw;
                _playerGameService.EndGame(request);
                ShowScores();

            }
            else
            if (_dealerScore > _playerScore && _dealerScore<=21 || _playerScore>21)
            {
                Console.WriteLine("Дилер победил!");
                request.ResultGame = EnumStatusGame.DealerWin;
                _playerGameService.EndGame(request);
                ShowScores();
            }
            else

            {
                Console.WriteLine("Игрок победил!");
                request.ResultGame = EnumStatusGame.DealerLoss;
                _playerGameService.EndGame(request);
                ShowScores();
            }



        }
        /// <summary>
        /// Показать очки дилера и игрока
        /// </summary>
        private void ShowScores()
        {
            _playerScore = GetHandScore(_playerHand);
            _dealerScore = GetHandScore(_dealerHand);
            Console.WriteLine($"У вас {_playerScore} очков. У диллера {_dealerScore} очков.");
        }
        
       
    }
}
