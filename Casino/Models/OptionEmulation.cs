using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Models
{
    public class OptionEmulation
    {
        /// <summary>
        /// Кол-во максимаьных очков которые берет бот
        /// </summary>
        public int BotMaxTake { get; set; } //= 18;
        /// <summary>
        /// Кол-во игр
        /// </summary>
        public int CountGames { get; set; } //= 100;
    }
}
