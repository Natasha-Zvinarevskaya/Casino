using Casino.DataContext.Enums;
using Casino.DataContext.Enums.BlackjackGame;
using Casino.DataContext.Models.BlackjackGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Services.Models
{
    public class PlayerModel
    {
        /// <summary>
        /// Список карт в руке игрока
        /// </summary>
        public List<Card> Cards { get; set; }
        /// <summary>
        /// Ид пользователя
        /// </summary>
        public int? UserId { get; set; }
        /// <summary>
        /// Очки игрока
        /// </summary>
        public int Score { get { return GetHandScore(Cards); } }
        /// <summary>
        /// Дилер или нет
        /// </summary>
        public bool IsDealer { get; set; }
        /// <summary>
        /// Статус игры
        /// </summary>
        public EnumStatusPlayerGame StatusGame { get; set; }
        // public bool IsHide {  get; set; }

        /// <summary>
        /// Сумма очков у заданной руки 
        /// </summary>
        /// <param name="cards">Карты в руке</param>
        /// <returns>Счет</returns>
        private int GetHandScore(List<Card> cards)
        {
            int aces = 0;
            int score = 0;
            foreach (var card in cards.Where(x => !x.IsHide))
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
        /// Скрыть карты 
        /// </summary>
        /// <returns></returns>
        public List<Card> HideCards()
        {
            var cards = new List<Card>();
             foreach (var card in Cards)
            {
                cards.Add(card.HideCard());
            }
             return cards;
            
        }
    }
}
