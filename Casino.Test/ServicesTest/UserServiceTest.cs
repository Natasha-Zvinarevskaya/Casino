using Casino.Services.Interfaces;
using Casino.Services.Models;
using Casino.Services.RequestResponse.UserService.Request;
using Casino.Services.Service;
using Casino.Test.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Test.ServicesTest
{
    public class UserServiceTest
    {
        [Fact]
        public void SaveUserImage_SaveFile()
        {
            //Arange
            //var mock = new Mock<IUserTransactionService>();
            //var contextOptions = GetContextWithData();

            //var service = new UserService(contextOptions, mock.Object);

            var service = ServerProviderTests.GetServerProvider();
            var userService = service.GetService<IUserService>();

            SaveUserImageRequest request1 = new SaveUserImageRequest { Image = ConvertImageToB64() };
            BaseUserIdReq<SaveUserImageRequest> request = new BaseUserIdReq<SaveUserImageRequest>(3, request1);


            var path = @"D:,,,\\Image\Users\\";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            //Act

            var result = userService.SaveUserImage(request);

            //Assert
            Assert.True(result.IsSucces);

        }
        private string ConvertImageToB64()
        {
            string image = Convert.ToBase64String(File.ReadAllBytes($"D:\\Натаха\\3a69aee66a3f324915ee3085baf9c6c4.jpg"));
            return image;

        }

        [Fact]
        public void ChangeName_NameEqualTom()
        {
            //Arange
            var serviceProvider = ServerProviderTests.GetServerProvider();

            var service = serviceProvider.GetService<IUserService>();



            ChangeUserNameRequest request1 = new ChangeUserNameRequest { UserId = 1, Name = "Tom" };
            BaseUserIdReq<ChangeUserNameRequest> request = new BaseUserIdReq<ChangeUserNameRequest>(1, request1);

            //Act
            var result1 = service.ChangeUserName(request);

            var result2 = service.GetUserData(1);

            //Assert
            Assert.True(result1.IsSucces);
            Assert.Equal("Tom", result2.Data.Name);
        }
    }
}
