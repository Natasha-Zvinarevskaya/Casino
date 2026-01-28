using Casino.DataContext.Enums.BlackjackGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Services.RequestResponse.BlackjackGame.Requests
{
    public class GetDealerSettingRequest
    {
        public EnumBlackJackGameSettings Id { get; set; }
        public string Settings {  get; set; }
    }
}
