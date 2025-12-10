using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Services.RequestResponse.GoogleAuth.Request
{
    public class OptionGoogleSettings
    {
        /// <summary>
        /// Публичный ключ гугла
        /// </summary>
        public string ClientId { get; set; }
        /// <summary>
        /// Секретный ключ гугла
        /// </summary>
        public string SecretKey { get; set; }
    }
}
