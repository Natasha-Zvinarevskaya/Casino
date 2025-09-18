using Casino.Services.Interfaces;
using Casino.Services.Models.BlackjackGame.Response;
using Casino.Services.Models.UserTransactionService.Request;
using Casino.Web.WebSockets;
using Microsoft.AspNetCore.Mvc;

namespace Casino.Web.Controllers.WsControllers
{
    public class BalanceWsController : WsController
    {
        private IUserTransactionService _userTransactionService;
        public BalanceWsController(IUserTransactionService userTransactionService)
        {
            _userTransactionService = userTransactionService;
        }

        public IActionResult Index()
        {
            return View();
        }
        public BaseResponse TopUpBalance(TopUpBalanceRequest request)
        {
            _userTransactionService.ReplenishmentBalance(request);
            return new BaseResponse ();

        }
    }
}
