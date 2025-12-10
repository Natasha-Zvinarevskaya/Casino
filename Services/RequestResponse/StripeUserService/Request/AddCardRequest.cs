using Casino.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Services.RequestResponse.StripeUserService.Request
{
    public class AddCardRequest
    {
       
        /// <summary>
        /// Емаил пользователя
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Полное имя пользователя
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        ///  Cvc код карты
        /// </summary>
        public string Cvc { get; set; }
        /// <summary>
        /// Год истечения срока действия карты
        /// </summary>
        public long ExpYear { get; set; }
        /// <summary>
        /// Месяц истечения срока действия карты
        /// </summary>
        public long ExpMonth { get; set; }
        /// <summary>
        /// Номер карты
        /// </summary>
        public string Number { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string AccountCard { get; set; }
      
    }
}
