using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Services.RequestResponse.StripeUserService.Response
{
    public class CreatePaymentMethodStripeResponse
    {
        /// <summary>
        /// Ид клиента страйп
        /// </summary>
        public string CustomerId { get; set; }
        /// <summary>
        /// Ид способа оплаты в страйп
        /// </summary>
        public string PaymentMetodId { get; set; }
        /// <summary>
        /// 4 последние цифры карты
        /// </summary>
        public string CardLast { get; set; }
        /// <summary>
        /// Бренд карты (visa, master и т.д.)
        /// </summary>
        public string CardType { get; set; }
    }
}
