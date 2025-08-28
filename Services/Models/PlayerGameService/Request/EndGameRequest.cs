using Casino.DataContext.Enums;
using Casino.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Services.Models.PlayerGameService.Request
{
    public class EndGameRequest
    {
        public EnumStatusGame ResultGame { get; set; }
        /// <summary>
        /// Ид игры
        /// </summary>
        public int GameId { get; set; }
    }
}