using Casino.Services.Interfaces;
using Casino.Services.Models;
using Casino.Services.RequestResponse.PlayerGameService.Request;
using Casino.Services.Service;
using Casino.Web.Middleware;
using Casino.Web.WebSockets;
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
        public List<int> GetUsersIds(BaseGameIdReq req)
        {
            var userIds = _userService.GetListUsersId(new BaseGameIdReq(req.GameId));

            return userIds.Data;
        }
        /// <summary>
        /// Подключение игрока к игре
        /// </summary>
        /// <param name="gameId">ид игры</param>
        /// <returns></returns>
        public BaseResponse ConnectPlayer(BaseGameIdReq req)
        {
            _playerGameService.ConnectPlayer(new BaseUserIdReq<int>(User.UserId, req.GameId));
            var userIds = _userService.GetListUsersId(new BaseGameIdReq(req.GameId));

            _webSocketManager.SendMessageToUser(new SendMessageRequest
            {
                CurrentUserId = User.UserId,
                UserIds = userIds.Data,
                Controller = nameof(GameWsController),
                Method = nameof(ConnectAnotherPlayer),
                Value = $"Connect Another Player. Id:{User.UserId}"
            });


            //if (_playerGameService.IsGameReady(new BaseGameIdReq(req.GameId)))
            //{

            //}
            return new BaseResponse();
        }
        /// <summary>
        /// Отключение игрока от игры
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns></returns>
        public BaseResponse DisconnectPlayer(BaseGameIdReq req)
        {

            _playerGameService.DisconnectPlayer(new BaseUserIdReq<int>(User.UserId, req.GameId));

            var userIds = _userService.GetListUsersId(new BaseGameIdReq(req.GameId));


            _webSocketManager.SendMessageAllUsers(new SendMessageRequest
            {
                CurrentUserId = User.UserId,
                UserIds = userIds.Data,
                Controller = nameof(GameWsController),
                Method = nameof(DisconnectAnotherPlayer),
                Value = $"DisconnectAnotherPlayer{User.UserId}."
            });

            return new BaseResponse();
        }
        /// <summary>
        /// Создание новой игры
        /// </summary>
        /// <param name="request">Ид игры и ставка</param>
        /// <returns></returns>
        public CreateGameResponce CreateGame(StartGameRequest request)
        {
            //_webSocketManager.SendMessageToUser(User.UserId, new NotificationRequest { Controller = nameof(GameWsController), Method = nameof(CreateGame), Value = $"Игра создана." });

            var gameId = _playerGameService.CreateGame(new BaseUserIdReq<StartGameRequest>(User.UserId, request));

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
