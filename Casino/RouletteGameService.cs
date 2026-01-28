using Casino.Services.Models.RouletteGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino
{
    public class RouletteGameService
    {
        private Roulette _roulette = new Roulette();
        public void Play()
        {
            List<Sector> gameRoulette = _roulette.Shuffle();
            Console.WriteLine("Выберите цвет, который победит: \n1. Красный. \n2. Черный. \n3. Зеленый.");
           var answerUser=Convert.ToInt32( Console.ReadLine());
            //if (answerUser>=1 && answerUser<=3)
            //{
                var droppSector = Turn(gameRoulette);
                droppSector.ToString(droppSector);
            //}
            //else 
            //    Console.WriteLine()
          
        }
        /// <summary>
        /// Ход шарика
        /// </summary>
        /// <param name="roulette">Рулетка</param>
        /// <returns>Ячейку, которая победила</returns>
        public Sector Turn(List<Sector> roulette)
        {
            Random rand = new Random();
            int start = rand.Next(0, roulette.Count);
            var test1 = "2565";
           
            var test = "pO";
           
            Sector droppSector = roulette[start];
            int i = rand.Next(10, 50);
            for (; i > 0;)
            {
                for (var j = start; j <= 37; j++)
                {
                    droppSector = roulette[j];
                    i--;
                    if (i == 0)
                        break;
                }
            }
            return droppSector;
        }
    }
}
