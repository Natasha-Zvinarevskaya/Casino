using Casino.DataContext.Enums.BlackjackGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Services.RequestResponse.BlackjackGame.Response
{
    public class GetDealerSettingResponse
    {
        //public EnumBlackJackGameSettings Id { get; set; }
        //public string Settings { get; set; }
        public int CountPoints { get; set; }
        /// <summary>
        /// Процент риска, на который пойдет диллер в случае, если очков меньшк 18
        /// </summary>
        public float PercentRisk { get; set; }
        /// <summary>
        /// Процент жульничества, когда диллер возьмет нужную карту
        /// </summary>
        public float PercentСheating { get; set; }
        /// <summary>
        /// Процент на сколько сильно он сжульничает и возьмет победную карту из колоды
        /// </summary>
        public float PercentWinCheating { get; set; }
    }
}
