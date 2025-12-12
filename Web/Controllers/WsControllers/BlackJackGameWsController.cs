using Casino.Services.Interfaces;
using Casino.Services.Models;
using Casino.Services.RequestResponse.BlackjackGame.Requests;
using Casino.Services.RequestResponse.BlackjackGame.Response;
using Casino.Services.Service;
using Casino.Web.WebSockets;
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
            return response;

        }
        /// <summary>
        /// Ход игрока
        /// </summary>
        /// <param name="">Ид игры</param>
        /// <returns>ид игры, статус игры (победа, проигрыш и т.д.), карты в руках дилера и игрока</returns>
        public BaseResponse<BlackJackGameModel> TurnPlayer(TurnPlayerRequest request)
        {
            var userIds = _userService.GetListUsersId(new BaseGameIdReq(request.GameId));
            if (userIds.Data.Any())
            {
                foreach (var userId in userIds.Data)
                {
                    _webSocketManager.SendMessageAllUsers(new SendMessageRequest()
                    {
                        CurrentUserId = User.UserId,
                        Controller = nameof(BlackJackGameWsController),
                        Method = nameof(TurnPlayer),
                        Value = $"TurnPlayer{User.UserId}"
                    });

                }
            }


            var response = _blackJackGameService.Turn(new TurnPlayerRequest { GameId = request.GameId, UserId = User.UserId });
            return response;
        }
        /// <summary>
        /// Пропуск хода игрока
        /// </summary>
        /// <param name="gameId">ИД игры</param>
        /// <returns> ид игры, статус игры, карты в руках игроков</returns>
        public BaseResponse<BlackJackGameModel> SkipPlayer(TurnPlayerRequest request)
        {
            var userIds = _userService.GetListUsersId(new BaseGameIdReq(request.GameId));
            if (userIds.Data.Any())
            {
                foreach (var userId in userIds.Data)
                {
                    _webSocketManager.SendMessageAllUsers(new SendMessageRequest()
                    {
                        CurrentUserId = User.UserId,
                        Controller = nameof(BlackJackGameWsController),
                        Method = nameof(SkipPlayer),
                        Value = $"TurnPlayer{User.UserId}"
                    });

                }
            }


            var response = _blackJackGameService.SkipPlayer(new BaseGameIdReq(request.GameId));
            return response;
        }
    }
}

