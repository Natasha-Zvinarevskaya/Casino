using Casino.DataContext.Enums;
using Casino.Services.Models;
using Casino.Services.Models.BlackjackGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Services.RequestResponse.BlackjackGame.Response
{
    public class BlackJackGameModel
    {
        public int GameId { get; set; }
        public EnumStatusGame Status { get; set; }
        public List<PlayerModel> PLayerCards { get; set; }
        public List<PlayerModel> HideCards(int ignoreUserId)
        {
            var playersCards = new List<PlayerModel>();
           
            foreach (var player in PLayerCards)
            {
                List<Card> cards = player.Cards;
                if (player.UserId != ignoreUserId)
                {
                     cards = player.HideCards();

                }
                playersCards.Add(new PlayerModel()
                {
                    IsDealer = player.IsDealer,
                    Cards = cards,
                    StatusGame = player.StatusGame,
                    UserId = player.UserId
                });

            }
            return playersCards;
        }


    }
}
