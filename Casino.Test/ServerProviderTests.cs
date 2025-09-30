using Casino.DataContext;
using Casino.Services.Interfaces;
using Casino.Services.Request.GoogleAuth;
using Casino.Services.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Test
{
    public static class ServerProviderTests
    {
        public static IServiceProvider GetServerProvider()
        {


            var services = new ServiceCollection();
            services.AddDbContext<CasinoDbContext>(options => options.UseInMemoryDatabase(Guid.NewGuid().ToString()));
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserSessionService, UserSessionService>();
            services.AddScoped<IBlackJackGameService, BlackjackService>();
            services.AddScoped<IPlayerGameService, PlayerGameService>();
            services.AddScoped<IUserTransactionService, UserTransactionService>();
            services.AddScoped<IGoogleService, GoogleServices>();


            //IConfiguration AppConfiguration;
           
            services.Configure<OptionGoogleSettings>(opt =>
            {
                opt.ClientId = "411996394677-cvua557j7cm6keqpk0sao5is1g42s7aq.apps.googleusercontent.com";
            });

           // services.Configure<OptionGoogleSettings>(services.Configuration.GetSection(nameof(OptionGoogleSettings)));
            //services.AddCors();


            var serviceProvider = services.BuildServiceProvider();
            var options = serviceProvider.GetService<DbContextOptions<CasinoDbContext>>();
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
            return serviceProvider;
        }

    }
}
