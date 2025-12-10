using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.DataContext
{
    /// <summary>
    /// Пользователь
    /// </summary>
    public class Users
    {
        /// <summary>
        /// Пользователя
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Почта
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Пароль
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Имя
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// Баланс
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal Balance { get; set; }


        public List<Game> Games { get; set; }

        public List<UserTransactions> Transactions { get; set; }
        public List<UserSession> UserSessions { get; set; }

        public UserProvider UserProvider { get; set; }

        public List <MoneyTransaction> MoneyTransactions { get; set; }
        public StripeCustomer StripeUser { get; set; }
        public List <Payment> Payments { get; set; }
        public List<UsersGame> UsersGames { get; set; }
    }
}
