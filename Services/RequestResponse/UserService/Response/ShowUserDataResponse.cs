using Casino.Services.RequestResponse.UserTransactionService.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Services.RequestResponse.UserService.Response
{
    public class ShowUserDataResponse
    {
        /// <summary>
        /// Ид пользователя
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// Емаил пользователя
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Баланс пользователя
        /// </summary>
        public decimal Balance { get; set; }
        /// <summary>
        /// История транзакций (кол-во сыгранных игр, кол-во побед ,проигрышей, ничьих, суммы выигранных и проиграных денег) пользователя
        /// </summary>
        public GetHistoryTransactionResponse HistoryTransaction { get; set; }
        /// <summary>
        /// Картинка пользователя
        /// </summary>
        public string Image { get; set; }
    }
}
