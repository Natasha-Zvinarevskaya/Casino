using Casino.DataContext;
using Casino.Services.Enums.BlackjackGame;
using Casino.Services.Interfaces;
using Casino.Services.Models;
using Casino.Services.Models.BlackjackGame;
using Casino.Services.RequestResponse.GoogleAuth.Request;
using Casino.Services.Service;
using Google.Apis.Drive.v3.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Stripe.Extension.Interfaces;
using Stripe.Extension.Models;
using Stripe.Extension.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Casino.Test.Helpers
{
    public static class ServerProviderTests
    {
        public static IServiceProvider GetServerProvider()
        {


            var services = new ServiceCollection();
            services.AddDbContext<CasinoDbContext>(options => options.UseInMemoryDatabase(Guid.NewGuid().ToString())
            //игнорирование запрет на транзакции в бд InMemory
            .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning)));
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserSessionService, UserSessionService>();
            services.AddScoped<IBlackJackGameService, BlackjackService>();
            services.AddScoped<IPlayerGameService, PlayerGameService>();
            services.AddScoped<IUserTransactionService, UserTransactionService>();
            services.AddScoped<IGoogleService, GoogleServices>();
            services.AddScoped<IStripeUserService, StripeUserService>();
            services.AddScoped<IStripePaymentServices, StripePaymentServices>();


            //IConfiguration AppConfiguration;

            services.Configure<OptionGoogleSettings>(opt =>
            {
                opt.ClientId = "411996394677-cvua557j7cm6keqpk0sao5is1g42s7aq.apps.googleusercontent.com";
            });
            services.Configure<StripeSettings>(opt =>
            {
                opt.SecretKey = "";
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
            context.UserProviders.Add(new UserProvider { Id = 1, ProviderType = DataContext.Enums.EnumProviderType.Google, User = user1, UserId = 1, Token = "k" });

            context.StripeCustomers.Add(new StripeCustomer { Id = 1, CustomerId = "cus_NbZ8Ki3f322LNn", AccountCard = "test", CardLast = "123", CardType = "visa", UserId = 1, PaymentMethodId = "pm_card_visa" });
            context.StripeCustomers.Add(new StripeCustomer { Id = 2, AccountCard = "test2", CardLast = "321", CardType = "visa", UserId = 2 });


            var cardsHistoryJson = new CardsHistoryJson
            {
                Players = new List<PlayerModel>
            { new PlayerModel
            { Cards = new List<Card> { new Card { Suit = CardSuit.Spades, Value = CardValue.King }, new Card { Suit = CardSuit.Diamonds, Value = CardValue.Ten }
            },
                UserId = 1,
                IsDealer = false,
                StatusGame = DataContext.Enums.EnumStatusGame.None } ,
                new PlayerModel
             {Cards=new List<Card> { new Card { Suit = CardSuit.Spades, Value = CardValue.Four }, new Card { Suit = CardSuit.Diamonds, Value = CardValue.Nine }
             },
                IsDealer = true,
                StatusGame = DataContext.Enums.EnumStatusGame.None } ,
                new PlayerModel
                {
                    Cards = new List<Card> { new Card { Suit = CardSuit.Spades, Value = CardValue.Jack }, new Card { Suit = CardSuit.Diamonds, Value = CardValue.Eight }
            },
                UserId = 2,
                IsDealer = false,
                StatusGame = DataContext.Enums.EnumStatusGame.None
                }
                },
                PlayersSkiped = 2,
                Deck = new List<Card> { new Card { Suit = CardSuit.Spades, Value = CardValue.Jack }, new Card { Suit = CardSuit.Diamonds, Value = CardValue.Eight } }

            };
            var history = JsonSerializer.Serialize(cardsHistoryJson);
            context.GameHistory.Add(new GameHistory { GameId =1, History = history });
            Game game1 = new Game
            {
                Status = DataContext.Enums.EnumStatusGame.WaitingPlayers,
                GameSettings = new GameSettings
                {
                    AmountWin = 2,
                    GameId = 1
                },
                UsersGames = new List<UsersGame>()
                {
                    new UsersGame { User = user1 },   
                    new UsersGame { User = user2 }

                }
            };
            context.Games.Add(game1);
         Game game2 = new Game
                 {
                Status = DataContext.Enums.EnumStatusGame.ReadyToGame,
                GameSettings = new GameSettings
                {
                    AmountWin = 2,
                    GameId = 2,
                    MaxCountPlayers = 2
                }
              // UsersGames = new List<UsersGame>()
            };
            context.Games.Add(game2);

            context.SaveChanges();
            return serviceProvider;
        }

    }
}
