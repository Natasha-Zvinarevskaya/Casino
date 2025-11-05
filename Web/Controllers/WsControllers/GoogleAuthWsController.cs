using Casino.Services.Interfaces;
using Casino.Services.Request.GoogleAuth;
using Casino.Web.WebSockets;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Linq;
using static System.Net.Mime.MediaTypeNames;
using static System.Net.WebRequestMethods;
using System.Text.Encodings.Web;
using Casino.DataContext;
using Google.Apis.Drive.v3.Data;

namespace Casino.Web.Controllers.WsControllers
{
    
    public class GoogleAuthWsController : WsController
    {
        private IGoogleService _googleService;
        public GoogleAuthWsController(IGoogleService googleService)
        {
            _googleService = googleService;
        }

        public IActionResult Index()
        {
            return View();
        }
       
        /// <summary>
        /// Авторизация через гугл
        /// </summary>
        /// <param name="request"></param>
        /// <returns>Ссылка на гугл авторизацию с данными о пользователе</returns>
        public string GetAuthUrl(GetAuthUrlRequest request)
        {
            var redirectUrl = _googleService.GoogleProvider(request);
           
            return redirectUrl;
        }
       

        

       

       
    }
}
