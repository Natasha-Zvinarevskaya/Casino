using Casino.DataContext.Enums;
using Casino.Services.Models;
using Casino.Services.Models.BlackjackGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Services.RequestResponse.BlackjackGame.Response
{
    public class BlackJackGameModel
    {
        public int GameId { get; set; }
        public EnumStatusGame Status { get; set; }
       // public List<Card> DealerCards { get;set; }
        public List<PlayerModel> PLayerCards { get; set; }

    }
}
