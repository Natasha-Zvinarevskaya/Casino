using Casino.Services.Interfaces;
using Casino.Services.Request.GoogleAuth;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using static System.Net.WebRequestMethods;

namespace Casino.Services.Service
{
    public class GoogleServices: IGoogleService
    {
        OptionGoogleSettings _options;
        public GoogleServices(IOptions<OptionGoogleSettings> options)
        {
            _options = options.Value;
        }

        /// <summary>
        /// Создается ссылка для авторизации через google
        /// </summary>
        /// <param name="request">ProviderType,RedirectUrl,Action,AuthToken</param>
        /// <returns></returns>
        public string GoogleProvider (GetAuthUrlRequest request)
         {
            string uri = $"https://accounts.google.com/o/oauth2/v2/auth?response_type=code&client_id={_options.ClientId}&redirect_uri={request.RedirectUrl}&scope=openid%20email%20profile&access_type=offline&prompt=consent&state={request.AuthToken},{request.ProviderType},{request.Action}&include_granted_scopes=true";
            var redirectUri=HttpUtility.UrlEncode(uri);
            return redirectUri;
        }
    }
}
