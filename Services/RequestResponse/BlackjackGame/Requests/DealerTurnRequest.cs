using Casino.Services.Models;
using Casino.Services.Models.BlackjackGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Services.RequestResponse.BlackjackGame.Requests
{
    public class DealerTurnRequest
    {
        /// <summary>
        /// Карты в руке дилера
        /// </summary>
        ///public PlayerModel DealerHand { get; set; }
        /// <summary>
        ///  Ид игры
        /// </summary>
        public int GameId { get; set; }
        /// <summary>
        /// Карты в колоде
        /// </summary>
        // public Deck Deck { get; set; }



    }
}
