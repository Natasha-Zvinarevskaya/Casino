using Casino.Services.Interfaces;
using Casino.Services.Models;
using Casino.Services.RequestResponse.UserService.Request;
using Casino.Services.RequestResponse.UserService.Response;
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

        /// <summary>
        /// Показать данные пользователя
        /// </summary>
        /// <returns></returns>
        public BaseResponse<ShowUserDataResponse> ShowUserData()
        {
            var response = _userService.GetUserData(User.UserId);
            return response;

        }
        /// <summary>
        /// Изменить имя пользователя
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public IActionResult ChangeUserName(ChangeUserNameRequest request)
        {
            _userService.ChangeUserName(new BaseUserIdReq<ChangeUserNameRequest>(User.UserId, request));
            return View();
        }
        /// <summary>
        /// Сохранить картинку в профиле пользователя
        /// </summary>
        /// <param name="request">Ид пользователя, картинка</param>
        /// <returns></returns>
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
