using Casino.DataContext.Enums;
using Casino.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Services.RequestResponse.PlayerGameService.Response
{
    public class GetHistoryResponse
    {
        /// <summary>
        /// Карты в игре
        /// </summary>
        public CardsHistoryJson CardsHistory { get; set; }
        /// <summary>
        /// Статус игры
        /// </summary>
        public EnumStatusGame StatusGame { get; set; }
        /// <summary>
        /// Пропустившие ход игроки
        /// </summary>
        public int PlayersSkiped { get; set; } 
    }
}
