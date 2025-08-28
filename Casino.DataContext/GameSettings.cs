using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.DataContext
{
    /// <summary>
    /// Настройки игры
    /// </summary>
    public class GameSettings
    {
        /// <summary>
        /// Игра
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Сумма выигрыша
        /// </summary>
        public decimal AmountWin { get; set; }

        public PlayerGame PlayerGame { get; set; }
        public int PlayerGameId { get; set; }


    }
}
