using Casino.Services.Interfaces;
using Casino.Services.Models;
using Casino.Services.RequestResponse.PlayerGameService.Request;
using Casino.Services.Service;
using Casino.Web.WebSockets;
using Microsoft.AspNetCore.Mvc;

namespace Casino.Web.Controllers.WsControllers
{
    public class GameWsController : WsController
    {
        IUserService _userService;
        private IPlayerGameService _playerGameService;

        public GameWsController(IUserService userService, IPlayerGameService playerGameService)
        {
            _userService = userService;
            _playerGameService = playerGameService;
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
        public List<int> GetUsersIds(int gameId)
        {
            var userIds = _userService.GetListUsersId(gameId);
           
            return userIds.Data;
        }
        /// <summary>
        /// Подключение игрока к игре
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns></returns>
        public bool ConnectPlayer(int gameId)
        {
            _playerGameService.ConnectPlayer(User.UserId, gameId);
            return _playerGameService.IsGameReady(gameId);
        }
        /// <summary>
        /// Отключение игрока от игры
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns></returns>
        public IActionResult DisconnectPlayer(int gameId)
        {
            _playerGameService.DisconnectPlayer(User.UserId, gameId);
            return View();
        }
        /// <summary>
        /// Создание новой игры
        /// </summary>
        /// <param name="request">Ид игры и ставка</param>
        /// <returns></returns>
        public IActionResult CreateGame(StartGameRequest request)
        {
            _playerGameService.CreateGame(new BaseUserIdReq<StartGameRequest>(User.UserId, request));
            return View();
        }

    }
}
