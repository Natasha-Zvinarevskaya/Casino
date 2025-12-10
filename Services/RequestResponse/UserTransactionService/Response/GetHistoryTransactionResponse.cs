using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Services.RequestResponse.UserTransactionService.Response
{
    /// <summary>
    /// Ответ,который выдаст метод GetHistoryTransaction
    /// </summary>
    public class GetHistoryTransactionResponse
    {
        /// <summary>
        /// Кол-во побед
        /// </summary>
        public int CountWins { get; set; }
        /// <summary>
        /// Кол-во проигрышей
        /// </summary>
        public int CountLosses { get; set; }
        /// <summary>
        /// Кол-во ничьих
        /// </summary>
        public int CountDraws { get; set; }
        /// <summary>
        /// Кол-во сыгранных игр
        /// </summary>
        public int CountGames { get; set; }
        /// <summary>
        /// Сумма потерянных денег при всех проигрышах
        /// </summary>
        public decimal AmountLost { get; set; }
        /// <summary>
        /// Сумма всех выигранных денег
        /// </summary>
        public decimal AmountWon { get; set; }
    }
}
