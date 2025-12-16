using Casino.Services.Models;
using Casino.Services.Models.BlackjackGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Services.RequestResponse.PlayerGameService.Request
{
    public class SaveGameHistoryRequest
    {
        public List<Card> Deck { get; set; }
        public List<PlayerModel> PlayersHands { get; set; }
       // public List<Card> DealerHand { get; set; }
        public int GameId { get; set; }
       // public int PlayersSkiped { get; set; }
    }
}
