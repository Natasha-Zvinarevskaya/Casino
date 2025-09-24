using Casino.Services.Models.UserTransactionService.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Services.Models.UserService.Response
{
    public class ShowUserDataResponse
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public decimal Balance { get; set; }
        public GetHistoryTransactionResponse HistoryTransaction { get; set; }
        public string Image { get; set; }
    }
}
