using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.DataContext.Enums
{
    /// <summary>
    /// Типы транзакций
    /// </summary>
    public enum EnumTypeTransaction
    {
        /// <summary>
        /// При победе
        /// </summary>
        Win,
        /// <summary>
        /// При проигрыше
        /// </summary>
        Loss,
        /// <summary>
        /// При ничьей
        /// </summary>
        Draw,
        /// <summary>
        /// Пополнение пользователем
        /// </summary>
        Replenishment
    }
}
