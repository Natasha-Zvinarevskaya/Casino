using Casino.Services.Interfaces;
using Casino.Services.Models.BlackjackGame.Response;
using Casino.Web.WebSockets;
using Microsoft.AspNetCore.Mvc;

namespace Casino.Web.Controllers.WsControllers
{
    public class GameWsController : WsController
    {
        IUserService _userService;
        public GameWsController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }
        public BaseResponse<List<int>> GetUsersIds(int gameId)
        {
            var userIds = _userService.GetListUsersId(gameId);
            return new BaseResponse<List<int>>(userIds.Data);
        }

    }
}
