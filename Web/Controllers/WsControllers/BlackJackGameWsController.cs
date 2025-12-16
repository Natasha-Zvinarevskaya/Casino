using Casino.Services.Interfaces;
using Casino.Services.Models;
using Casino.Services.Models.Notifications;
using Casino.Services.RequestResponse.BlackjackGame.Requests;
using Casino.Services.RequestResponse.BlackjackGame.Response;
using Casino.Services.RequestResponse.UserService.Request;
using Casino.Services.Service;
using Casino.Web.WebSockets.Models;
using Microsoft.AspNetCore.Mvc;

namespace Casino.Web.Controllers.WsControllers

{
    public class BlackJackGameWsController : WsController
    {
        private IBlackJackGameService _blackJackGameService;
        private WebSockets.WebSocketManager _webSocketManager;
        private IUserService _userService;

        public BlackJackGameWsController(
            IBlackJackGameService blackJackGameService,
            WebSockets.WebSocketManager webSocketManager,
            IUserService userService)
        {
            _blackJackGameService = blackJackGameService;
            _webSocketManager = webSocketManager;
            _userService = userService;
        }


        /// <summary>
        /// Начало игры 
        /// </summary>
        /// <param name="request">Ставка пользователя</param>
        /// <returns>ид игры, статус игры (победа, проигрыш и т.д.), карты в руках дилера и игрока</returns>
        public BaseResponse<BlackJackGameModel> StartGame(BlackjackPlayRequest request)
        {
            var response = _blackJackGameService.Play(request);
            //Уведомление пользователям
            var userIds = _userService.GetListUsersId(new GetListUsersIdsRequest { GameId = request.GameId });
            if (userIds.Data.Any())
            {
                foreach (var userId in userIds.Data)
                {
                    _webSocketManager.SendMessageSelectedUsers(new SendMessageRequest<StartGameNotification> ()
                    {
                        CurrentUserId = User.UserId,
                        Controller = nameof(BlackJackGameWsController),
                        Method = nameof(StartGame),
                        Value = new StartGameNotification { GameModel = response.Data }
                    });
                }
            }
           
            return response;
        }
        /// <summary>
        /// Ход игрока
        /// </summary>
        /// <param name="">Ид игры</param>
        /// <returns>ид игры, статус игры (победа, проигрыш и т.д.), карты в руках дилера и игрока</returns>
        public BaseResponse<BlackJackGameModel> TurnPlayer(TurnPlayerRequest request)
        {
            var response = _blackJackGameService.Turn(new TurnPlayerRequest { GameId = request.GameId, UserId = User.UserId });

            //Уведомление пользователям
            var userIds = _userService.GetListUsersId(new GetListUsersIdsRequest { GameId = request.GameId });
            if (userIds.Data.Any())
            {
                foreach (var userId in userIds.Data)
                {
                    _webSocketManager.SendMessageSelectedUsers(new SendMessageRequest<TurnPlayerNotification>()
                    {
                        CurrentUserId = User.UserId,
                        Controller = nameof(BlackJackGameWsController),
                        Method = nameof(TurnPlayer),
                        Value = new TurnPlayerNotification { UserId = User.UserId }
                    });

                }
            }
            return response;
        }
        /// <summary>
        /// Пропуск хода игрока
        /// </summary>
        /// <param name="gameId">ИД игры</param>
        /// <returns> ид игры, статус игры, карты в руках игроков</returns>
        public BaseResponse<BlackJackGameModel> SkipPlayer(SkipPlayerRequest request)
        {
            var response = _blackJackGameService.SkipPlayer(new SkipPlayerRequest { GameId = request.GameId });

            var userIds = _userService.GetListUsersId(new GetListUsersIdsRequest { GameId = request.GameId });
            if (userIds.Data.Any())
            {
                foreach (var userId in userIds.Data)
                {
                    _webSocketManager.SendMessageSelectedUsers(new SendMessageRequest <SkipPlayerNotification>()
                    {
                        CurrentUserId = User.UserId,
                        Controller = nameof(BlackJackGameWsController),
                        Method = nameof(SkipPlayer),
                       Value = new SkipPlayerNotification { UserId = User.UserId }
                       
                    });
                }
            }

            if ((int)response.Data.Status == 6)
            {
                foreach (var userId in userIds.Data)
                {
                    _webSocketManager.SendMessageSelectedUsers(new SendMessageRequest<GameOverNotification>()
                    {
                        CurrentUserId = User.UserId,
                        Controller = nameof(BlackJackGameWsController),
                        Method = nameof(SkipPlayer),
                        Value = new GameOverNotification { GameModel = response.Data}
                    });
                }
            }

            return response;
        }
    }
}

