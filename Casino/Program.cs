using Casino.DataContext;
using Casino.DataContext.Enums;
using Casino.Interfaces;
using Casino.Models;
using Casino.Services;
using Casino.Services.Interfaces;
using Casino.Services.RequestResponse.BlackjackGame.Requests;
using Casino.Services.RequestResponse.GoogleAuth.Request;
using Casino.Services.RequestResponse.PlayerGameService.Request;
using Casino.Services.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Casino
{
    internal class Program
    {
        public static void Main()
        {

            //        ///ПОдключение appsetings к проекту
            //        var configuration = new ConfigurationBuilder()
            //.AddJsonFile("appsettings.json")
            //.Build();

            //var services = new ConfigurationService().Init(configuration);

            //var playerGameService = services.GetService<IPlayerGameService>();
            //var blackjackService = services.GetService<IBlackJackGameService>();

            //Звсунуть все это в конфигурацию и использовать Инит


            //var playerGameService = services.BuildServiceProvider().GetService<IPlayerGameService>();
            //var blackjackService = services.BuildServiceProvider().GetService<IBlackJackGameService>();

            var configuration = new ConfigurationBuilder()
   .AddJsonFile("appsettings.json")
   .Build();
            var services = new ServiceCollection();
            services.AddScoped<IBlackJackGameService, BlackjackService>();
            services.AddScoped<IPlayerGameService, PlayerGameService>();
            services.AddScoped<IUserTransactionService, UserTransactionService>();
            services.AddDbContext<CasinoDbContext>(options => options.UseSqlServer(configuration.GetConnectionString(nameof(CasinoDbContext))));
            services.AddScoped<IUserService, UserService>();
            services.AddOptions();
            services.AddScoped<IEmulationBlackjackService, EmulationBlackjackService>();

            services.Configure<OptionEmulation>(configuration.GetSection(nameof(OptionEmulation)));

            var emulationBlackjackService = services.BuildServiceProvider().GetService<IEmulationBlackjackService>();


            while (true)
            {

                //Console.WriteLine("Выберите игру из списка и введите ее номер: \n1. Блэкджек.");
                //var answerUser = Convert.ToInt32(Console.ReadLine());
                //var request = new StartGameRequest() {  Game = EnumGames.BlackJack};
                //var gameId = playerGameService.StartGame(request);

                //new BlackjackGameService(playerGameService).Play(new BlackjackPlayRequest() { });//GameId = gameId });
                //Console.WriteLine("Чтобы сыграть снова нажмите любую кнопку.");
                //Console.ReadLine();

                //Console.WriteLine("Сколько максимально игр должно пройти в эмуляторе:");
                //var countGames = Convert.ToInt32(Console.ReadLine());
                //Console.WriteLine("Сколько максимально очков набирает бот:");
                //var botMaxTake = Convert.ToInt32(Console.ReadLine());
                //Console.WriteLine("");
                 emulationBlackjackService.StartEmulation();



            }
        }
    }
}
