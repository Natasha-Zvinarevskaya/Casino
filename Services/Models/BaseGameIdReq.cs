using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Services.Models
{
    public class BaseGameIdReq 
    { 
        public BaseGameIdReq (int gameId)
        {
            GameId = gameId;
        }
        public int GameId { get; set; }
    }
}
