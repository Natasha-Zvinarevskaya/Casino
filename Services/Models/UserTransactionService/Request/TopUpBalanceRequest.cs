using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Services.Models.UserTransactionService.Request
{
    /// <summary>
    /// Запрос для метода TopUpBalance
    /// </summary>
    public class TopUpBalanceRequest
    {
        /// <summary>
        /// Ид пользователя
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// Сумма для пополнения баланса
        /// </summary>
        public decimal Count { get; set; }
    }
}
