using Casino.DataContext.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Services.Models.PlayerGameService.Response
{
    public class GetHistoryResponse
    {
        public CardsHistoryJson CardsHistory { get; set; }
        public EnumStatusGame StatusGame { get; set; }
    }
}
