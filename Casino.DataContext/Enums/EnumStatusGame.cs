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
        /// Ожидание игроков
        /// </summary>
        WaitingPlayers,
        /// <summary>
        /// Готово к игре
        /// </summary>
        ReadyToGame,
        /// <summary>
        /// Конец игры
        /// </summary>
        GameOver


    }
}
