using Casino.Services.Enums.RouletteGame;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Services.Models.RouletteGame
{
    public class Sector
    {
        /// <summary>
        /// Число сектора
        /// </summary>
        public int Value { get; set; }
        /// <summary>
        /// Цвет сектора
        /// </summary>
        public SectorColor Color { get; set; }
        /// <summary>
        /// Показывает значение и масть карты
        /// </summary>
        /// <returns></returns>
        public void ToString(Sector sector)
        {
            Console.WriteLine($"Победило  {sector.Value} of {sector.Color} ");
        }
    }
}
