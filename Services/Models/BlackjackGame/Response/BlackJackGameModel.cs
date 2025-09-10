using Casino.DataContext.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Services.Models.BlackjackGame.Response
{
    public class BlackJackGameModel
    {
        public int GameId { get; set; }
        public EnumStatusGame Status { get; set; }
        public List<Card> DealerCards { get;set; }
        public List<Card> PlayerCards { get; set; }

    }
}
