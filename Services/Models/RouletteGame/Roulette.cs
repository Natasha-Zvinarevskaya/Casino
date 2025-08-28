using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Services.Models.RouletteGame
{
    public class Roulette
    {
       /// <summary>
       /// Список со всеми ячейками в рулетке
       /// </summary>
        private List<Sector> _roulette;
        /// <summary>
        /// Конструктор создания рулетки
        /// </summary>
        public Roulette()
        {
            _roulette.Add(new Sector { Value = 0, Color = Enums.RouletteGame.SectorColor.Green });
            for (var i = 1; i<=36; i++)
            {
                
                if (i<=10 || i >=19 && i<=28) //В диапазоне от 1 до 10 и от 19 до 28
                {
                    if (i % 2 == 0)
                    
                        _roulette.Add(new Sector { Value = i, Color = Enums.RouletteGame.SectorColor.Black }); // Четные числа черные
                    
                    else
                        _roulette.Add(new Sector { Value = i, Color = Enums.RouletteGame.SectorColor.Red }); //Нечетные красные
                }
                else //Иначе - порядок меняется
                {
                    if (i % 2 == 0)

                        _roulette.Add(new Sector { Value = i, Color = Enums.RouletteGame.SectorColor.Red }); // Четные числа красные
                    else
                        _roulette.Add(new Sector { Value = i, Color = Enums.RouletteGame.SectorColor.Black }); //Нечетные числа черные
                }
            }

        } 
        /// <summary>
        /// Метод перемешивания порядка секторов
        /// </summary>
        /// <returns></returns>
        public List<Sector> Shuffle()
        {
            Random rnd = new Random();
            _roulette = _roulette.OrderBy(c => rnd.Next()).ToList();
            return _roulette;
        }



    }
}
