using Casino.Services.Models.BlackjackGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Services.Models.PlayerGameService.Request
{
    public class SaveGameHistoryRequest
    {
        public List<Card> Deck { get; set; }
        public List<Card> PlayerHand { get; set; }
        public List<Card> DealerHand { get; set; }
        public int GameId { get; set; }
    }
}
