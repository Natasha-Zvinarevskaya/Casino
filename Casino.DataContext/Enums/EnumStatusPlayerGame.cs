using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.DataContext.Enums
{
    public enum EnumStatusPlayerGame
    {
        None,
        /// <summary>
        /// Победа
        /// </summary>
        Win,
        /// <summary>
        /// Проигрыш
        /// </summary>
        Loss,
        /// <summary>
        /// Ничья 
        /// </summary>
        Draw,
        /// <summary>
        /// Ожидание конца игры
        /// </summary>
        WaitingEndGame
    }
}
