using Casino.DataContext.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.DataContext
{
    /// <summary>
    /// Игра
    /// </summary>
    public class PlayerGame
    {
        /// <summary>
        /// Id игры
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Время начала игры
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// Статус игры
        /// </summary>
        public EnumStatusGame Status { get; set; }
        /// <summary>
        /// Время окончания игры
        /// </summary>
        public DateTime? DateEnd { get; set; }
        /// <summary>
        /// Сумма ставки
        /// </summary>
        public decimal AmountBet { get; set; }
        /// <summary>
        /// Какая игра из доступных выбрана
        /// </summary>
        public EnumGames Game { get; set; }


        public Users User { get; set; }
        public int UserId { get; set; }

        public GameSettings GameSettings { get; set; }
    }
}
