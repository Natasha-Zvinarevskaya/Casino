using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Models.RequestResponse.EmulationBlackjackService.Request
{
    public class StartEmulationRequest
    {
        /// <summary>
        /// Кол-во максимаьных очков которые берет бот
        /// </summary>
        public int BotMaxTake { get; set; }
        /// <summary>
        /// Кол-во игр
        /// </summary>
        public int CountGames { get; set; }
    }
}
