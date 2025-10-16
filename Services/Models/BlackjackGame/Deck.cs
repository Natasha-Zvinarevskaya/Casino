using Casino.Services.Enums.BlackjackGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Services.Models.BlackjackGame
{
    //Пример инкапсуляции
    /// <summary>
    /// Класс Колода
    /// </summary>
   public class Deck
    {
        /// <summary>
        /// Список со всеми картами колоды
        /// </summary>
        private List<Card> _cards { get; set; }
        /// <summary>
        /// Конструктор создания новой колоды
        /// </summary>
        public Deck ()
        {
            _cards = new List<Card>(); 
            foreach (CardValue value in Enum.GetValues(typeof(CardValue)))
            {
                foreach (CardSuit suit in Enum.GetValues(typeof(CardSuit)))
                {

                    _cards.Add(new Card { Suit = suit, Value = value });
                }
            }

        }
        /// <summary>
        /// Метод перетасовки колоды
        /// </summary>
        /// <returns>Возвращает перемешанную колоду</returns>
        public List<Card> Shuffle ()
        {
            Random rand = new Random();
            _cards=_cards.OrderBy(c => rand.Next()).ToList();
            return _cards;
        }
        /// <summary>
        /// Метод раздачи карты
        /// </summary>
        /// <returns>Отдать карту</returns>
        public Card DealCard()
        {
            var card = _cards[0];
            _cards.RemoveAt(0);
            return card;

        }
        /// <summary>
        /// Вернуть колоду
        /// </summary>
        /// <returns></returns>
        public List<Card> GetCards ()
        {
            return _cards;
        }
        /// <summary>
        /// Колода взятая из истории игры 
        /// </summary>
        /// <param name="cards"></param>
        public void HistoryDeck(List<Card> cards)
        {
            _cards = cards;
        }
    }
}
