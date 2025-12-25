using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.DataContext.Enums
{
    /// <summary>
    /// Статус игры
    /// </summary>
    public enum EnumStatusGame
    {
        /// <summary>
        /// Не существует
        /// </summary>
        None,
        /// <summary>
        /// Победа дилера
        /// </summary>
        DealerWin,
        /// <summary>
        /// Победа игрока
        /// </summary>
        DealerLoss,
        /// <summary>
        /// Ничья 
        /// </summary>
        Draw,
        /// <summary>
        /// Ожидание игроков
        /// </summary>
        WaitingPlayers,
        /// <summary>
        /// Готово к игре
        /// </summary>
        ReadyToGame
        




    }
}
