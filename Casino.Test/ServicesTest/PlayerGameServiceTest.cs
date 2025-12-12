using Casino.Services.Interfaces;
using Casino.Services.Models;
using Casino.Services.RequestResponse.PlayerGameService.Request;
using Casino.Test.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Test.ServicesTest
{
    public class PlayerGameServiceTest
    {
        [Fact]
        public void ConnectPlayer_IsSuccesTrue()
        {
            //Arrange 
            var service = ServerProviderTests.GetServerProvider();
            var playerGameService = service.GetService<IPlayerGameService>();

            var gameId = 2;
            var userId = 1;

            //Act
            var result = playerGameService.ConnectPlayer(new BaseUserIdReq <int> (userId, gameId));

            //Assert
            Assert.Equal(true, result.IsSucces);

        }

        [Fact]
        public void DisconnectPlayer_IsSuccesTrue()
        {
            //Arrange 
            var service = ServerProviderTests.GetServerProvider();
            var playerGameService = service.GetService<IPlayerGameService>();

            var gameId = 1;
            var userId = 1;

            //Act
            var result = playerGameService.DisconnectPlayer(new BaseUserIdReq<int>(userId, gameId));

            //Assert
            Assert.Equal(true, result.IsSucces);

        }
        [Fact]
        public void StartGame_Equal1()
        {
            //Arrange 
            var service = ServerProviderTests.GetServerProvider();
            var playerGameService = service.GetService<IPlayerGameService>();

            var request = new StartGameRequest() { Game = DataContext.Enums.EnumGames.BlackJack, Bet = 100, MaxCountPlayers = 3 };

            //Act
            var result = playerGameService.StartGame(request);

            //Assert
            Assert.Equal(3, result);
        }

    }
}

