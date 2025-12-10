using Casino.Services;
using Casino.Services.Interfaces;
using Casino.Services.RequestResponse.BlackjackGame.Requests;
using Casino.Services.RequestResponse.PlayerGameService.Request;
using Casino.Services.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Casino
{
    internal class Program
    {
        public static void Main()
        {

            ///ПОдключение бд к проекту
            var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

            var services = new ConfigurationService().Init(configuration);

            var playerGameService = services.GetService<IPlayerGameService>();


            while (true)
            {

                Console.WriteLine("Выберите игру из списка и введите ее номер: \n1. Блэкджек.");
                var answerUser = Convert.ToInt32(Console.ReadLine());
                var request = new StartGameRequest() {  Game = answerUser };
                var gameId = playerGameService.StartGame(request);

                new BlackjackGameService(playerGameService).Play(new BlackjackPlayRequest() { });//GameId = gameId });
                Console.WriteLine("Чтобы сыграть снова нажмите любую кнопку.");
                Console.ReadLine();
            }
        }
    }
}
