using Casino.Services.RequestResponse.BlackjackGame.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Services.Models.Notifications
{
    public class StartGameNotification
    {
        public BlackJackGameModel Data {  get; set; }
    }
}
