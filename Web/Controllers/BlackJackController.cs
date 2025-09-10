using Casino.Services.Interfaces;
using Casino.Services.Models.BlackjackGame.Requests;
using Casino.Services.Models.BlackjackGame.Response;
using Casino.Services.Models.PlayerGameService.Request;
using Casino.Web.WebSockets;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Casino.Web.Controllers
{
    [Route("[controller]")]
    public class BlackJackController : Controller
    {
        

        public IActionResult Index()
        {
            return View();
        }
        
    }
}
