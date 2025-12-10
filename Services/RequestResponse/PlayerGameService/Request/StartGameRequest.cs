using Casino.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Services.RequestResponse.PlayerGameService.Request
{
    public class StartGameRequest
    {
        //public int UserId { get; set; }
        public int Game { get; set; }
        public decimal Bet { get; set; }
        public int MaxCountPlayers { get; set; }

    }
}
