using Casino.Services.Enums.BlackjackGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Services.Models.BlackjackGame
{
    /// <summary>
    /// Класс Карта. Как должна выглядеть она карта
    /// </summary>
    public class Card
    {
        /// <summary>
        /// Масть карты
        /// </summary>
        public CardSuit? Suit { get; set; }
        /// <summary>
        /// Значение карты 
        /// </summary>
        public CardValue? Value { get; set; }
        public bool IsHide {  get; set; }

        /// <summary>
        /// Показывает значение и масть карты
        /// </summary>
        /// <returns></returns>
        public void ToString(Card card)
        {
            Console.WriteLine($"{card.Value} of {card.Suit} ");
        }
        public Card HideCard ()
        {
            var card = new Card ();
            card.IsHide = true;
            card.Suit = null;
            card.Value = null;
            return card;
           
        }
    }
}
