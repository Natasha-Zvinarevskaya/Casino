using Casino.DataContext;
using Casino.Services.Interfaces;
using Casino.Services.Models;
using Casino.Services.RequestResponse.PlayerGameService.Request;
using Casino.Web.WebSockets.Models;
using Microsoft.AspNetCore.Mvc;

namespace Casino.Web.Controllers.WsControllers
{
    public class PlayerGameWsController : WsController
    {
        private IPlayerGameService _playerGameService;
        public PlayerGameWsController(IPlayerGameService playerGameService)
        {
            _playerGameService = playerGameService;
        }
        public IActionResult Index()
        {
            return View();
        }
        
    }
}
