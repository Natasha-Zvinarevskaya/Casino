using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Models.RequestResponse.EmulationBlackjackService.Response
{
    public class DealerStatisticsResponse
    {
        public bool Risk { get; set; }
        public bool Cheating { get; set; }
        public bool WinCheating { get; set; }
        public bool IsCrook { get; set; }
    }
}
