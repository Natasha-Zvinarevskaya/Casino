using Casino.Services.Interfaces;
using Casino.Services.RequestResponse.BlackjackGame.Requests;
using Casino.Services.RequestResponse.PlayerGameService.Request;
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
    public class BlackjackServiceTest
    {
        [Fact]
        public void Play_NotNull()
        {
            //Arange 
            var service = ServerProviderTests.GetServerProvider();
            var blackjackService = service.GetService<IBlackJackGameService>();


            List<int> userIds = [3, 4];
            BlackjackPlayRequest request = new BlackjackPlayRequest() { UserIds = userIds, Bet = 100, GameId = 1};
           

            //Act 
            var result = blackjackService.Play(request);

            //Assert
            Assert.NotNull(result);

        }

        [Fact]
        public void Play_PlayersHave2Cards()
        {
            //Arange 
            var service = ServerProviderTests.GetServerProvider();
            var blackjackService = service.GetService<IBlackJackGameService>();


            List<int> userIds = [3, 4];
            BlackjackPlayRequest request = new BlackjackPlayRequest() { UserIds = userIds, Bet = 100, GameId = 1 };


            //Act 
            var result = blackjackService.Play(request);

            //Assert
            Assert.Equal(3, result.Data.PLayerCards.Count);

        }


    }
}
