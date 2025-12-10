using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Services.RequestResponse.UserTransactionService.Request
{
    /// <summary>
    /// Запрос для метода TopUpBalance
    /// </summary>
    public class TopUpBalanceRequest
    {
     
        /// <summary>
        /// Сумма для пополнения баланса
        /// </summary>
        public long Count { get; set; }
    }
}
