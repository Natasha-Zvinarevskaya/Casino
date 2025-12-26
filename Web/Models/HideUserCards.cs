using Casino.DataContext;
using Casino.Services.Models;
using Casino.Services.Models.Notifications;
using Casino.Services.RequestResponse.BlackjackGame.Response;
using Casino.Web.Controllers.WsControllers;
using Casino.Web.WebSockets.Models;
using Google.Apis.Drive.v3.Data;
using System.Net.WebSockets;

namespace Casino.Web.Models
{
    public class HideUserCards
    {
        private WebSockets.WebSocketManager _webSocketManager;
        public HideUserCards(WebSockets.WebSocketManager webSocketManager)
        {
            _webSocketManager = webSocketManager;
        }
        public void HideCards(SendMessageRequest<BlackJackGameModel> sendMessageRequest)
        {
            BlackjackGameHelper blackjackGameHelper = new BlackjackGameHelper();
           
            foreach (var userId in sendMessageRequest.UserIds)
            {
              var response=  blackjackGameHelper.HideUserCards(sendMessageRequest.Value , userId ) ;
                if(userId != sendMessageRequest.CurrentUserId)
                _webSocketManager.SendMessageToUser(new SendMessageRequest<BaseResponse<BlackJackGameModel>>
                {
                    Controller = sendMessageRequest.Controller,
                    Method = sendMessageRequest.Method,
                    Value = new BaseResponse<BlackJackGameModel> (response),
                    UserIds = new List<int> { userId }
                });
            }

        }
    }
}
