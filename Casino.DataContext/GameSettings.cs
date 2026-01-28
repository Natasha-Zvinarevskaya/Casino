using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
        [Column(TypeName = "decimal(18,2)")]
        public decimal AmountWin { get; set; }
        /// <summary>
        /// Максимальное кол-во игроков
        /// </summary>
        public int MaxCountPlayers { get; set; }

        public Game Game { get; set; }
        public int GameId { get; set; }

        
        


    }
}
