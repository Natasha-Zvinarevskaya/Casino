using Casino.DataContext;
using Casino.Services.Interfaces;
using Casino.Services.RequestResponse.BlackjackGame.Requests;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Casino.Web.Controllers.WsControllers;
using Casino.Test.Helpers;

namespace Casino.Test.WsControllersTest
{
    public class BlackJackGameWsControllerTest
    {
        [Fact]
        public void StartGame_PlayersCardCountEqual4()
        {
            //Arrange 
            var services = ServerProviderTests.GetServerProvider();
            var blackjackService = services.GetService<IBlackJackGameService>();
            var BlackJackGameWsController = new BlackJackGameWsController(blackjackService);
            var request = new BlackjackPlayRequest { GameId = 5, Bet = 10, UserIds = new List<int> { 1, 2, 3 } };

            //Act
            var response = BlackJackGameWsController.StartGame(request);

            //Assert
            Assert.Equal(4, response.Data.PLayerCards.Count);

        }

        [Fact]
        public void SkipPlayer_PlayersCardCountEqual3()
        {
            //Arrange 
            var services = ServerProviderTests.GetServerProvider();
            var blackjackService = services.GetService<IBlackJackGameService>();
            var BlackJackGameWsController = new BlackJackGameWsController(blackjackService);
            var request = new TurnPlayerRequest { gameId = 1 };
            var db = services.GetService<CasinoDbContext>();

            //Act
            var response = BlackJackGameWsController.SkipPlayer(request);

            //Assert
            Assert.Equal(3, response.Data.PLayerCards.Count);
        }
    }
}
