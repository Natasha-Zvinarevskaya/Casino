using Casino.Services.Interfaces;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Linq;
using static System.Net.Mime.MediaTypeNames;
using static System.Net.WebRequestMethods;
using System.Text.Encodings.Web;
using Casino.Services.RequestResponse.GoogleAuth.Request;

namespace Casino.Web.Controllers.APIControllers
{
    [ApiController]
    [Route("[controller]")]
    public class GoogleAuth : Controller
    {
        private IGoogleService _googleService;
        public GoogleAuth(IGoogleService googleService)
        {
            _googleService = googleService;
        }
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Оповещение от гугла
        /// </summary>
        /// <returns></returns>

        [Route("CallBack")]
        [HttpGet]
        public async Task<string> CallBack([FromQuery]CallbackGoogleRequest request)
        { 
            var IdToken = await _googleService.GetTokenGoogle(request);
            var email = _googleService.GetEmail(IdToken);

            return email.Data.Email;
        }

      

    
    }
}
