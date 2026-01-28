using Casino.Services.Interfaces;
using Casino.Services.Models;
using Casino.Services.RequestResponse.BlackjackGame.Response;
using Casino.Services.Service;
using Google.Apis.Drive.v3.Data;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using LoginRequest = Casino.Services.RequestResponse.Users.Request.LoginRequest;
using RegisterRequest = Casino.Services.RequestResponse.Users.Request.RegisterRequest;

namespace Casino.Web.Controllers.APIControllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : Controller
    {
        
        private IUserService _userService;
        public AuthController(IUserService userService)
        {
            _userService = userService;
        }
       
        /// <summary>
        /// Регистрация пользователя
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Register")]
        public IActionResult Register ( RegisterRequest user)
        {
            var userId = _userService.Registration(user);
            return Json(userId);
        }

        /// <summary>
        /// Вход пользователя в систему
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Login")]
        public IActionResult Login (LoginRequest user)
        {
            if (ModelState.IsValid)
            {
                var loginResponse = _userService.Login(user);
                if (loginResponse.IsSucces)
                {
                    Request.HttpContext.Response.Cookies.Append("Auth-Token", loginResponse.Data.Token.ToString());
                  
                    return Json(new BaseResponse());
                }
            }
            return Json(new BaseResponse("Ошибка."));
        }
        

        public IActionResult Index ()
        {
            return View();
        }
        ///// <summary>
        ///// Создание супер админа
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("RegisterSAdmin")]
        //public IActionResult RegistrationAdmin()
        //{
        //    var userId = _userService.RegistrationAdmin();
        //    return Json(userId);
        //}

    }
}
