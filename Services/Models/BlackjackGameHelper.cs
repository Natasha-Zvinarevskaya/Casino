using Azure;
using Casino.Services.RequestResponse.BlackjackGame.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Services.Models
{
    public class BlackjackGameHelper
    {
        public BlackJackGameModel HideUserCards(BlackJackGameModel blackJackGameModel, int userId)
        {
            var playersCards = blackJackGameModel.HideCards(userId);
            var response = new BlackJackGameModel()
            {
                GameId = blackJackGameModel.GameId,
                PLayerCards = playersCards,
                Status = blackJackGameModel.Status
            };
            return response;
        }
    }
}
