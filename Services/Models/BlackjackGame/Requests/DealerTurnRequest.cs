using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Services.Models.BlackjackGame.Requests
{
    public class DealerTurnRequest
    {
        /// <summary>
        /// Карты в руке дилера
        /// </summary>
        public List<Card> DealerHand { get; set; }
        /// <summary>
        /// Карты в колоде
        /// </summary>
        public Deck Deck { get; set; }
    }
}
