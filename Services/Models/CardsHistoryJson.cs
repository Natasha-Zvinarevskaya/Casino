using Casino.Services.Models.BlackjackGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Services.Models
{
    /// <summary>
    /// Модель , которая нужна для сериализации и десериализации , чтобы сайт читал отправленные ему данные
    /// </summary>
   public class CardsHistoryJson
    {
        /// <summary>
        /// колода
        /// </summary>
        public List<Card> Deck { get; set; }
        /// <summary>
        /// карты игрока
        /// </summary>
        public List<Card> PlayerHand { get; set; }
        /// <summary>
        /// карты дилера
        /// </summary>
        public List<Card> DealerHand { get; set; }

    }
}
