using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Services.RequestResponse.StripeUserService.Request
{
    public class SaveStripeCustomerCardRequest
    {
        /// <summary>
        /// 
        /// </summary>
        public string AccountCard { get; set; }
        /// <summary>
        /// Ид клиента страйп
        /// </summary>
        public string CustomerId { get; set; }
        /// <summary>
        /// Ид способа оплаты
        /// </summary>
        public string PaymentMethodId { get; set; }
        /// <summary>
        /// Последние 4 цифры номера карты
        /// </summary>
        public string CardLast { get; set; }
        /// <summary>
        /// Брэнд карты (visa, master и т.д.)
        /// </summary>
        public string CardType { get; set; }
    }
}
