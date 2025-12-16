using Casino.Services.Interfaces;
using Casino.Services.Models;
using Casino.Services.RequestResponse.UserTransactionService.Request;
using Casino.Web.WebSockets.Models;
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
        /// <summary>
        /// Пополнение баланса
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public BaseResponse TopUpBalance(BaseUserIdReq<TopUpBalanceRequest> request)
        {
           var response= _userTransactionService.ReplenishmentBalance(request);
            return response;

        }
    }
}
