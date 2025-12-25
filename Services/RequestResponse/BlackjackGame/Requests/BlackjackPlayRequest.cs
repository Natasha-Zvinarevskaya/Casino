using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Services.RequestResponse.BlackjackGame.Requests
{
    public class BlackjackPlayRequest
    {
        /// <summary>
        /// Ид игры
        /// </summary>
        public int GameId { get; set; }
        /// <summary>
        /// Сумма ставки
        /// </summary>
      //  public decimal Bet { get; set; }
        /// <summary>
        /// Список Ид пользователей
        /// </summary>
      //  public List<int> UserIds { get; set; }
    }
}
