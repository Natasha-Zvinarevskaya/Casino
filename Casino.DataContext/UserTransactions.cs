using Casino.DataContext.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.DataContext
{
    /// <summary>
    /// Транзакции пользователя
    /// </summary>
    public class UserTransactions
    {
        public int Id { get; set; }
        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// Тип
        /// </summary>
        public EnumTypeTransaction Type { get; set; }
        /// <summary>
        /// Сумма
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// Игра
        /// </summary>
        public int PlayerGame { get; set; }


        public int UsersId { get; set; }
        public Users User { get; set; }
    }
}
