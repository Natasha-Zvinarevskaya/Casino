using Casino.DataContext;
using Casino.Services.RequestResponse.UserTransactionService.Request;
using Casino.Services.Service;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Test.ServicesTest
{
    public class UserTransationServiceTest
    {
        [Fact]
        public void GetHistoryTransaction_Result()
        {
            //Arange
            var contextOptions = GetContextWithData();
            var service = new UserTransactionService(contextOptions);
            int userId = 1;

            //Act
            var result = service.GetHistoryTransactions(new GetHistoryTransactionsRequest { UserId = userId });

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
            context.Users.Add(user2);

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


    }
}
