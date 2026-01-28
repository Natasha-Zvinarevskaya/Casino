using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.DataContext.Enums.BlackjackGame
{
    public enum EnumBlackJackGameSettings
    {
        /// <summary>
        /// Кол-во очков, которое набирает диллер стандартно
        /// </summary>
        CountPoints,
        /// <summary>
        /// Процент риска, на который пойдет диллер в случае, если очков меньшк 18
        /// </summary>
        PercentRisk,
        /// <summary>
        /// Процент жульничества, когда диллер возьмет нужную карту
        /// </summary>
        PercentСheating,
        /// <summary>
        /// Процент на сколько сильно он сжульничает и возьмет победную карту из колоды
        /// </summary>
        PercentWinCheating
    }
}
