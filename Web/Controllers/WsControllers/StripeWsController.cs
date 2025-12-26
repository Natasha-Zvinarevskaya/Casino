using Casino.Services.Interfaces;
using Casino.Services.Models;
using Casino.Services.RequestResponse.StripeUserService.Request;
using Casino.Services.RequestResponse.UserTransactionService.Request;
using Casino.Services.Service;
using Casino.Web.WebSockets.Models;
using Microsoft.AspNetCore.Mvc;
using Stripe.Extension.Interfaces;
using Stripe.Extension.Models.StripePaymentServices.Request;

namespace Casino.Web.Controllers.WsControllers
{

    public class StripeWsController : WsController
    {
        private IStripeUserService _stripeUserService;
        private IUserTransactionService _userTransactionService;
        public StripeWsController(IStripeUserService stripeUserService, IUserTransactionService userTransactionService)
        {
            _stripeUserService = stripeUserService;
            _userTransactionService = userTransactionService;
        }
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Создать клиента страйп и привязать ему способ оплаты, записать все в бд
        /// </summary>
        /// <param name="request">Ид пользователя, маил , полной имя, cvc, номер карты, месяц и год истечения, аккаунт карты</param>
        /// <returns></returns>
        public IActionResult AddCard(BaseUserIdReq<AddCardRequest> request)
        {
            var response = _stripeUserService.AddCard(request);
            return View();
        }
        /// <summary>
        /// Пополнение баланса
        /// </summary>
        /// <param name="request">Ид пользователя, сумма поплнения </param>
        /// <returns></returns>
        public IActionResult BalanceReplenishment(BaseUserIdReq<TopUpBalanceRequest> request)
        {
            var response = _stripeUserService.TopUpBalance(request);
            return View();
        }

        public BaseResponse<decimal> TestBalanceReplenishment (TopUpBalanceRequest request)
        {
            _userTransactionService.ReplenishmentBalance(new BaseUserIdReq<TopUpBalanceRequest>(User.UserId, new TopUpBalanceRequest() { Count = request.Count }));
            return new BaseResponse<decimal>(request.Count);
        }



    }
}
