using Moq;
using Casino.Services.Interfaces;
using Casino.Services.Service;
using Casino.Services.Models.BlackjackGame.Requests;
using Casino.Services.Models.PlayerGameService.Request;
using Casino.DataContext;
using Microsoft.EntityFrameworkCore;
using Casino.Services.Models.UserService.Request;
using System.IO;
using Casino.Services.Models;
using Casino.Services.Models.BlackjackGame.Response;
using Casino.Services.Models.UserTransactionService.Response;
using Microsoft.Extensions.DependencyInjection;
using Casino.Services.Request.GoogleAuth;
using Microsoft.Extensions.Options;

namespace Casino.Test
{


    public class CasinoTest
    {
        [Fact]
        public void Blackjack_Play()
        {
            //Arange 
            var mock = new Mock<IPlayerGameService>();
            var service = new BlackjackService(mock.Object);

            int userId = 3;
            BlackjackPlayRequest request = new BlackjackPlayRequest();
            request.Bet = 100;
            //var startRequest = new StartGameRequest { Bet = request.Bet, UserId = userId, Game = 1 };
            mock.Setup(x => x.StartGame(It.IsAny<StartGameRequest>())).Returns<StartGameRequest>(x => 1);

            //Act 
            var result = service.Play(request, userId);

            //Assert
            Assert.NotNull(result);

        }
        [Fact]
        public void Blackjack_Play_PlayersHave2Cards()
        {
            //Arange 
            var mock = new Mock<IPlayerGameService>();
            var service = new BlackjackService(mock.Object);

            int userId = 3;
            BlackjackPlayRequest request = new BlackjackPlayRequest();
            request.Bet = 100;
            //var startRequest = new StartGameRequest { Bet = request.Bet, UserId = userId, Game = 1 };
            mock.Setup(x => x.StartGame(It.IsAny<StartGameRequest>())).Returns<StartGameRequest>(x => 1);

            //Act 
            var result = service.Play(request, userId);

            //Assert
            Assert.Equal(2, result.Data.PLayerCards.Count);
            Assert.Equal(2, result.Data.DealerCards.Count);

        }

        [Fact]
        public void UserTransationService_GetHistoryTransaction_Result()
        {
            //Arange
            var contextOptions = GetContextWithData();
            var service = new UserTransactionService(contextOptions);
            int userId = 1;

            //Act
            var result = service.GetHistoryTransactions(userId);

            //Assert 
            Assert.Equal(3, result.Data.CountWins);
            Assert.Equal(7, result.Data.CountGames);
            Assert.Equal(2, result.Data.CountDraws);
            Assert.Equal(400, result.Data.AmountWon);
        }

        private DbContextOptions<CasinoDbContext> GetContextWithData()
        {
            var options = new DbContextOptionsBuilder<CasinoDbContext>()
                 .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var context = new CasinoDbContext(options);
            var user1 = new Users { Id = 1, Balance = 100, Email = "user1@user1.user1", Name = "User1", Password = "111" };
            var user2 = new Users { Id = 2, Balance = 200, Email = "user2@user2.user2", Name = "User2", Password = "222" };

            context.Users.Add(user1);

            context.UserTransactions.Add(new UserTransactions { Type = DataContext.Enums.EnumTypeTransaction.Win, Amount = 100, UsersId = 1, User = user1 });
            context.UserTransactions.Add(new UserTransactions { Type = DataContext.Enums.EnumTypeTransaction.Win, Amount = 200, UsersId = 1, User = user1 });
            context.UserTransactions.Add(new UserTransactions { Type = DataContext.Enums.EnumTypeTransaction.Loss, Amount = 100, UsersId = 1, User = user1 });
            context.UserTransactions.Add(new UserTransactions { Type = DataContext.Enums.EnumTypeTransaction.Win, Amount = 100, UsersId = 1, User = user1 });
            context.UserTransactions.Add(new UserTransactions { Type = DataContext.Enums.EnumTypeTransaction.Loss, Amount = 200, UsersId = 1, User = user1 });
            context.UserTransactions.Add(new UserTransactions { Type = DataContext.Enums.EnumTypeTransaction.Draw, Amount = 100, UsersId = 1, User = user1 });
            context.UserTransactions.Add(new UserTransactions { Type = DataContext.Enums.EnumTypeTransaction.Draw, Amount = 100, UsersId = 1, User = user1 });
            context.UserTransactions.Add(new UserTransactions { Type = DataContext.Enums.EnumTypeTransaction.Draw, Amount = 100, UsersId = 2 });
            context.SaveChanges();
            return options;
        }

        [Fact]
        public void UserService_SaveUserImage_SaveFile()
        {
            //Arange
            var mock = new Mock<IUserTransactionService>();
            var contextOptions = GetContextWithData();

            var service = new UserService(contextOptions, mock.Object);
            SaveUserImageRequest request1 = new SaveUserImageRequest { UserId = 3, Image = ConvertImageToB64() };
            BaseUserIdReq<SaveUserImageRequest> request = new BaseUserIdReq<SaveUserImageRequest>(3, request1);


            var path = @"D:,,,\\Image\Users\\";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            //Act

            var result = service.SaveUserImage(request);

            //Assert
            Assert.True(result.IsSucces);

        }
        public string ConvertImageToB64()
        {
            string image = Convert.ToBase64String(File.ReadAllBytes($"D:\\Натаха\\3a69aee66a3f324915ee3085baf9c6c4.jpg"));
            return image;

        }

        [Fact]
        public void UserService_ChangeName_NameEqualTom()
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

        [Fact]
        public void GoogleService_GetAuthUrl_resultNotNull()
        {
            //Arange
            var serviceProvider = ServerProviderTests.GetServerProvider();
            var service = serviceProvider.GetService<IGoogleService>();

            GetAuthUrlRequest request = new GetAuthUrlRequest { ProviderType = 111, RedirectUrl = "http://localhost:5179/Blackjack", Action = 222, AuthToken = 333 };

            //Act
            var result = service.GoogleProvider(request);

            //Assert
            Assert.NotNull(result);
        }
    }
}