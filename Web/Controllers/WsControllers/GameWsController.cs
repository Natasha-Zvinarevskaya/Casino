using Casino.DataContext;
using Casino.Services.Interfaces;
using Casino.Services.Models;
using Casino.Services.Models.Notifications;
using Casino.Services.RequestResponse.PlayerGameService.Request;
using Casino.Services.RequestResponse.UserService.Request;
using Casino.Services.Service;
using Casino.Web.Middleware;
using Casino.Web.WebSockets.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Protocol;

namespace Casino.Web.Controllers.WsControllers
{
    public class GameWsController : WsController
    {
        private IUserService _userService;
        private IPlayerGameService _playerGameService;
        private WebSockets.WebSocketManager _webSocketManager;

        public GameWsController(IUserService userService, IPlayerGameService playerGameService, WebSockets.WebSocketManager webSocketManager)
        {
            _userService = userService;
            _playerGameService = playerGameService;
            _webSocketManager = webSocketManager;
        }

        public IActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// Получить Список Ид Пользователей
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns></returns>
        public List<int> GetUsersIds(GetListUsersIdsRequest req)
        {
            var userIds = _userService.GetListUsersId(new GetListUsersIdsRequest { GameId = req.GameId });
            return userIds.Data;
        }
        /// <summary>
        /// Подключение игрока к игре
        /// </summary>
        /// <param name="gameId">ид игры</param>
        /// <returns></returns>
        public BaseResponse ConnectPlayer(ConnectPlayerRequest req)
        {
           var response = _playerGameService.ConnectPlayer(new BaseUserIdReq<ConnectPlayerRequest>(User.UserId, new ConnectPlayerRequest { GameId = req.GameId, Bet = req.Bet }));
            
            var userIds = _userService.GetListUsersId(new GetListUsersIdsRequest { GameId = req.GameId });
            _webSocketManager.SendMessageSelectedUsers(new SendMessageRequest<ConnectPlayerNotification>
            {
                CurrentUserId = User.UserId,
                UserIds = userIds.Data,
                Controller = nameof(GameWsController),
                Method = nameof(ConnectAnotherPlayer),
                Value = new ConnectPlayerNotification { UserId = User.UserId}
            });
            return response;
        }
        /// <summary>
        /// Отключение игрока от игры
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns></returns>
        public BaseResponse DisconnectPlayer(GetListUsersIdsRequest req)
        {

           var response = _playerGameService.DisconnectPlayer(new BaseUserIdReq<int>(User.UserId, req.GameId));

            var userIds = _userService.GetListUsersId(new GetListUsersIdsRequest { GameId = req.GameId });
            _webSocketManager.SendMessageSelectedUsers(new SendMessageRequest <DisconnectPlayerNotification>
            {
                CurrentUserId = User.UserId,
                UserIds = userIds.Data,
                Controller = nameof(GameWsController),
                Method = nameof(DisconnectAnotherPlayer),
                Value = new DisconnectPlayerNotification { UserId = User.UserId}
            });

            return response;
        }
        /// <summary>
        /// Создание новой игры
        /// </summary>
        /// <param name="request">Ид игры и ставка</param>
        /// <returns></returns>
        public CreateGameResponce CreateGame(StartGameRequest request)
        {
            var gameId = _playerGameService.CreateGame(new BaseUserIdReq<StartGameRequest>(User.UserId, request));
            _webSocketManager.SendMessageAllUsers(new SendMessageRequest<CreateGameNotification>
            {
                CurrentUserId = User.UserId,
              
                Controller = nameof(GameWsController),
                Method = nameof(CreateGame),
                Value = new CreateGameNotification { GameId = gameId.GameId }
            });
             _playerGameService.ConnectPlayer(new BaseUserIdReq<ConnectPlayerRequest>(User.UserId, new ConnectPlayerRequest { GameId = gameId.GameId, Bet = request.Bet }));

            return (gameId);
          
        }
        public BaseResponse DisconnectAnotherPlayer()
        {
            return new BaseResponse();
        }
        public BaseResponse ConnectAnotherPlayer()
        {
            return new BaseResponse();
        }

    }
}
