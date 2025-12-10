using Casino.Services.Models;
using Casino.Services.RequestResponse.StripeUserService.Request;
using Casino.Services.RequestResponse.StripeUserService.Response;
using Casino.Services.RequestResponse.UserTransactionService.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Services.Interfaces
{
    public interface IStripeUserService
    {
        BaseResponse SaveStripeCustomerCard(BaseUserIdReq<SaveStripeCustomerCardRequest> request);
        BaseResponse<GetStripeCustomerResponse> GetStripeCustomer(GetStripeCustomerRequest request);
        BaseResponse<CreatePaymentMethodStripeResponse> AddCard(BaseUserIdReq<AddCardRequest> request);
        BaseResponse TopUpBalance(BaseUserIdReq<TopUpBalanceRequest> request);

       // BaseResponse<int> Test(CreatePaymentMethodRequest request);

    }
}
