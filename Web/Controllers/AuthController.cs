using Casino.Services.Interfaces;
using Casino.Services.Models;
using Casino.Services.Models.BlackjackGame.Response;
using Casino.Services.Request.Users;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using LoginRequest = Casino.Services.Request.Users.LoginRequest;
using RegisterRequest = Casino.Services.Request.Users.RegisterRequest;

namespace Casino.Web.Controllers
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
       

        [HttpPost]
        [Route("Register")]
        public IActionResult Register ( RegisterRequest user)
        {
            var userId = _userService.Registration(user);
            return Json(userId);
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login (LoginRequest user)
        {
            if (ModelState.IsValid)
            {
                var loginResponse = _userService.Login(user);
                if (loginResponse.IsSucces)
                {
                    //Request.HttpContext.Session.SetString("Auth-Token", loginResponse.Data.Token.ToString());
                    //context.Response.Cookies.Append("name", "Tom");

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

    }
}
