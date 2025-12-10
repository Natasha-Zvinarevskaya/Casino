using Casino.Services.Interfaces;
using Casino.Services.Models;
using Casino.Services.RequestResponse.StripeUserService.Request;
using Casino.Services.RequestResponse.UserTransactionService.Request;
using Casino.Test.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Test.ServicesTest
{
    public class StripeUserServiceTest
    {
        [Fact]
        public void AddCard_NotNull()
        {
            //Arange 
            var serviceProvider = ServerProviderTests.GetServerProvider();
            var service = serviceProvider.GetService<IStripeUserService>();


            var request = new BaseUserIdReq<AddCardRequest>(3,
                 new AddCardRequest()
                 {
                     AccountCard = "test",
                     //Cvc = "123",
                     Email = "test@test.test",
                     ExpMonth = 12,
                     ExpYear = 26,
                     FullName = "test",
                     Number = "4242424242424242"
                 });

            //Act
            var result = service.AddCard(request);

            //Assert 
            Assert.NotNull(result);
        }

        [Fact]
        public void TopUpBalance_Equal_IsSuccesTrue()
        {
            //Arange
            var services = ServerProviderTests.GetServerProvider();
            var stripeUserService = services.GetService<IStripeUserService>();

            var request = new BaseUserIdReq<TopUpBalanceRequest>(1, new TopUpBalanceRequest { Count = 5000 });

            //Act
            var response = stripeUserService.TopUpBalance(request);

            //Assert
            Assert.Equal(true, response.IsSucces);

        }
    }
}
