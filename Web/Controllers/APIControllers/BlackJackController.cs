
using Microsoft.AspNetCore.Mvc;

namespace Casino.Web.Controllers.APIControllers
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
