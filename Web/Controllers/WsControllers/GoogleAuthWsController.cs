using Casino.Services.Interfaces;
using Casino.Services.Request.GoogleAuth;
using Casino.Web.WebSockets;
using Microsoft.AspNetCore.Mvc;

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

        public string GetAuthUrl(GetAuthUrlRequest request)
        {
            var redirectUrl=_googleService.GoogleProvider(request);
            
            return redirectUrl;
        }
    }
}
