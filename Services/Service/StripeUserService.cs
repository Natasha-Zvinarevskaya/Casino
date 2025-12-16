using Casino.DataContext;
using Casino.Services.Interfaces;
using Casino.Services.Models;
using Casino.Services.Models.BlackjackGame;
using Casino.Services.RequestResponse.StripeUserService.Request;
using Casino.Services.RequestResponse.StripeUserService.Response;
using Casino.Services.RequestResponse.UserTransactionService.Request;
using Casino.Services.Service;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Stripe.Extension.Interfaces;
using Stripe.Extension.Models.StripePaymentServices.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Casino.Services.Service
{
    public class StripeUserService : IStripeUserService
    {
        private DbContextOptions<CasinoDbContext> _options;
        private IStripePaymentServices _stripePaymentService;
        private IUserTransactionService _userTransactionService;
        public StripeUserService(
            DbContextOptions<CasinoDbContext> options,
            IStripePaymentServices stripePaymentService,
            IUserTransactionService userTransactionService)
        {
            _options = options;
            _stripePaymentService = stripePaymentService;
            _userTransactionService = userTransactionService;
        }
        /// <summary>
        /// Сохранить карту клиента страйп в бд
        /// </summary>
        /// <param name="request">AccountCard, Ид клиента страйп, Ид способа оплаты, Последние 4 цифры карты и тип карты</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public BaseResponse SaveStripeCustomerCard(BaseUserIdReq<SaveStripeCustomerCardRequest> request)
        {
            var db = new CasinoDbContext(_options);
            var stripeUser = db.StripeCustomers.FirstOrDefault(x => x.UserId == request.UserId);
            if (stripeUser == null)
            {
                db.StripeCustomers.Add(new StripeCustomer()
                {
                    AccountCard = request.Request.AccountCard,
                    CustomerId = request.Request.CustomerId,
                    PaymentMethodId = request.Request.PaymentMethodId,
                    CardLast = request.Request.CardLast,
                    CardType = request.Request.CardType,

                });
            }
            else
            {
                stripeUser.AccountCard = request.Request.AccountCard;
                stripeUser.CustomerId = request.Request.CustomerId;
                stripeUser.PaymentMethodId = request.Request.PaymentMethodId;
                stripeUser.CardLast = request.Request.CardLast;
                stripeUser.CardType = request.Request.CardType;
            }
            db.SaveChanges();
            return new BaseResponse();

        }

        /// <summary>
        /// Получить информацию Stripe клиента по Ид пользователя
        /// </summary>
        /// <param name="request">Ид пользователя</param>
        /// <returns>Модель Stripe клиента</returns>
        /// <exception cref="Exception"></exception>
        public BaseResponse<GetStripeCustomerResponse> GetStripeCustomer(GetStripeCustomerRequest request)
        {
            var db = new CasinoDbContext(_options);
            var stripeUser = db.StripeCustomers.FirstOrDefault(x => x.UserId == request.UserId);
            if (stripeUser == null)
            {
                return null;
            }
            var response = new GetStripeCustomerResponse()
            {
                Id = stripeUser.Id,
                DateCreate = stripeUser.DateCreate,
                CardLast = stripeUser.CardLast,
                PaymentMethodId = stripeUser.PaymentMethodId,
                CustomerId = stripeUser.CustomerId,
                UserId = stripeUser.UserId,
                AccountCard = stripeUser.AccountCard,
                CardType = stripeUser.CardType

            };
            return new BaseResponse<GetStripeCustomerResponse>(response);
        }
      
        //public BaseResponse<int> Test (CreatePaymentMethodRequest request)
        //{
        //    var response =_stripePaymentService.Test(request);
        //    return new BaseResponse<int>(response.Id);  
        //}
        public BaseResponse<CreatePaymentMethodStripeResponse> AddCard(BaseUserIdReq<AddCardRequest> request)
        {

            var customer = GetStripeCustomer(new GetStripeCustomerRequest
            {
                UserId = request.UserId
            });
            var customerId = "";
            if (customer == null)
            {
                var customerResponse = _stripePaymentService.CreateCustomer(new CreateCutomerRequest()
                {
                    Email = request.Request.Email,
                    Name = request.Request.FullName,
                    Description = request.UserId.ToString()
                });
                if (!customerResponse.IsSucces) throw new Exception("Пользователь не найден.");
                customerId = customerResponse.Data.CustomerId;
            }
            else
            {
                if (!string.IsNullOrEmpty(customer.Data.PaymentMethodId)) throw new Exception("Карта уже привязана к аккаунту");
                customerId = customer.Data.CustomerId;
            }

            //var paymentMethod = _stripePaymentService.CreatePaymentMethod(new CreatePaymentMethodRequest()
            //{
            //    Cvc = request.Cvc,
            //    ExpMonth = request.ExpMonth,
            //    ExpYear = request.ExpYear,
            //    Number = request.Number
            //});

            //  if (!paymentMethod.IsSucces) throw new Exception(paymentMethod.ErrorMessage);

            var paymentMethodAttach = _stripePaymentService.AttachPaymentMethodToCustomer(new AttachPaymentMethodToCustomerRequest()
            {
                CustomerId = customerId,
                PaymentMethodId = "pm_card_visa"
            });

            if (!paymentMethodAttach.IsSucces) throw new Exception(paymentMethodAttach.ErrorMessage);


            var paymentMethodUserId = SaveStripeCustomerCard(new BaseUserIdReq<SaveStripeCustomerCardRequest>
                (request.UserId, new SaveStripeCustomerCardRequest()
                {
                    AccountCard = request.Request.AccountCard,
                    CustomerId = customerId,
                    PaymentMethodId = "pm_card_visa"
                    //CardLast = paymentMethod.Data.Last4,
                    // CardType = paymentMethod.Data.Brand
                }
                ));
            return new BaseResponse<CreatePaymentMethodStripeResponse>(new CreatePaymentMethodStripeResponse()
            {
                CustomerId = customerId,
                PaymentMetodId = "pm_card_visa"
                // CardLast = paymentMethod.Data.Last4,
                //CardType = paymentMethod.Data.Brand
            });

        }
       
        /// <summary>
        /// Пополнение баланса через страйп
        /// </summary>
        /// <param name="request">Ид пользователя, сумма пополнения</param>
        /// <returns></returns>
        /// <exception cref="Exception">Пользователь не найден в бд клиентов страп</exception>
        public BaseResponse TopUpBalance(BaseUserIdReq<TopUpBalanceRequest> request)
        {
            //Находим страйп клиента в базе данных
            var customer = GetStripeCustomer(new GetStripeCustomerRequest() { UserId = request.UserId });
            if (customer == null) throw new Exception("Пользователь не найден");

            //Создаем запрос на платеж
            _stripePaymentService.CreatePaymentIntent(new CreatePaymentIntentRequest()
            {
                Amount = request.Request.Count,
                PaymentMethodId = customer.Data.PaymentMethodId
            });

            //Пополняем баланс пользователя в игре на введенную сумму
            var response = _userTransactionService.ReplenishmentBalance(request);

            return response;
        }
    }
}
