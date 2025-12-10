using Casino.Services.Interfaces;
using Casino.Services.RequestResponse.GoogleAuth.Request;
using Casino.Test.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Test.ServicesTest
{
    public class GoogleServiceTest
    {
        [Fact]
        public void GetAuthUrl_resultNotNull()
        {
            //Arange
            var serviceProvider = ServerProviderTests.GetServerProvider();
            var service = serviceProvider.GetService<IGoogleService>();

            GetAuthUrlRequest request = new GetAuthUrlRequest { ProviderType = 111, RedirectUrl = "http://localhost:5179/GoogleAuth/CallBack", Action = 222, AuthToken = 333 };

            //Act
            var result = service.GoogleProvider(request);

            //Assert
            Assert.NotNull(result);

        }

        [Fact]
        public async Task GetTokenGoogle_resultNotNull()
        {
            //Arange
            var serviceProvider = ServerProviderTests.GetServerProvider();
            var service = serviceProvider.GetService<IGoogleService>();
            CallbackGoogleRequest request = new CallbackGoogleRequest { Code = "c" };

            //Act

            var result = await service.GetTokenGoogle(request);

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void GetEmail_result()
        {
            //Arange
            var serviceProvider = ServerProviderTests.GetServerProvider();
            var service = serviceProvider.GetService<IGoogleService>();
            ResponseGetToken request = new ResponseGetToken
            {
                access_token = "at",
                expires_in = 3599,
                id_token = "t",
                refresh_token = "k",
                scope = "openid ",
                token_type = "Bearer"
            };

            //Act
            var result = service.GetEmail(request.id_token);

            //Assert
            Assert.NotNull(result);
        }
        [Fact]
        public void GoogleRegister_IsSucsses()
        {
            //Arange
            var serviceProvider = ServerProviderTests.GetServerProvider();
            var service = serviceProvider.GetService<IGoogleService>();
            GoogleRegisterRequest request = new GoogleRegisterRequest { Email = "ex@gmail.com", Name = "Exx", Token = "k" };

            //Act
            var result = service.GoogleRegister(request);

            //Assert
            Assert.Equal(true, result.IsSucces);
        }
        [Fact]
        public void GoogleLogin_NotNull()
        {
            //Arange
            var serviceProvider = ServerProviderTests.GetServerProvider();
            var service = serviceProvider.GetService<IGoogleService>();

            var request = new GoogleLoginRequest { Email = "user1@user1.user1", Token = "k" };

            //Act
            var result = service.GoogleLogin(request);

            //Assert
            Assert.NotNull(result.Data);
        }
    }
}
