
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.DataContext
{
    public class StripeCustomer
    {
        /// <summary>
        /// Ид 
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Ид клиента Stripe
        /// </summary>
        public string CustomerId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string AccountCard { get; set; }
        /// <summary>
        /// Ид способа оплаты
        /// </summary>
        public string PaymentMethodId { get; set; }
        /// <summary>
        /// Последние четыре цифры карты
        /// </summary>
        public string CardLast { get; set; }
        /// <summary>
        /// Брэнд карты (visa, master и т.д.)
        /// </summary>
        public string CardType { get; set; }
        /// <summary>
        /// Дата создания 
        /// </summary>
        public DateTime DateCreate { get; set; }

        /// <summary>
        /// Ид пользователя
        /// </summary>
        public int UserId { get; set; }


        public Users User { get; set; }
    }
}
