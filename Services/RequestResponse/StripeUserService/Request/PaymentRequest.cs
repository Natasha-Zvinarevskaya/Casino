using Casino.DataContext.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Services.RequestResponse.StripeUserService.Request
{
    public class PaymentRequest
    {
        public decimal Amount { get; set; }
        public int UserId { get; set; }
       
    }
}
