using Casino.DataContext.Models.BlackjackGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Services.RequestResponse.BlackjackGame.Response
{
    public class DealerTurnResponse
    {
        public int DealerScore { get; set; }
        public List<Card> DealerHand { get; set; }
        public List<Card> Deck { get; set; }
    }
}
