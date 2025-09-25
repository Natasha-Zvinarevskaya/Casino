using Casino.Services.Interfaces;
using Casino.Services.Models;
using Casino.Services.Models.BlackjackGame.Response;
using Casino.Services.Models.UserService.Request;
using Casino.Services.Models.UserService.Response;
using Casino.Web.WebSockets;
using Microsoft.AspNetCore.Mvc;

namespace Casino.Web.Controllers.WsControllers
{
    public class UserWsController : WsController
    {
        private IUserService _userService;
        public UserWsController(IUserService userService)
        {
            _userService = userService;
        }

        public BaseResponse<ShowUserDataResponse> ShowUserData()
        {
            var response = _userService.GetUserData(User.UserId);
            return response;

        }
        public IActionResult ChangeUserName(ChangeUserNameRequest request)
        {

            _userService.ChangeUserName(new BaseUserIdReq<ChangeUserNameRequest>(User.UserId, request));
            return View();

        }
        public IActionResult SaveUserImage(SaveUserImageRequest request)
        {
            _userService.SaveUserImage(new BaseUserIdReq<SaveUserImageRequest>(User.UserId, request));
            return View();
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
