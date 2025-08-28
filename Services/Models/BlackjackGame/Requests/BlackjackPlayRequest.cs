using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Services.Models.BlackjackGame.Requests
{
    public class BlackjackPlayRequest
    {
        /// <summary>
        /// Ид игры
        /// </summary>
        public int GameId { get; set; }
    }
}
