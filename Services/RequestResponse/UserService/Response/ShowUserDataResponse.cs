using Casino.Services.RequestResponse.UserTransactionService.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Services.RequestResponse.UserService.Response
{
    public class ShowUserDataResponse
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public decimal Balance { get; set; }
        public GetHistoryTransactionResponse HistoryTransaction { get; set; }
        public string Image { get; set; }
    }
}
