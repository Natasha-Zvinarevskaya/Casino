using Casino.DataContext.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Services.RequestResponse.UserTransactionService.Request
{
    public class EndGameTransactionRequest
    {
        public int UserId { get; set; }
        public EnumStatusPlayerGame StatusGame { get; set; }
        public int GameId { get; set; }
    }
}
