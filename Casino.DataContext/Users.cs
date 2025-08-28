using System;
using System.Collections.Generic;
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
        public decimal Balance { get; set; }


        public List<PlayerGame> PlayerGames { get; set; }

        public List<UserTransactions> Transactions { get; set; }
        public List<UserSession> UserSessions { get; set; }
    }
}
