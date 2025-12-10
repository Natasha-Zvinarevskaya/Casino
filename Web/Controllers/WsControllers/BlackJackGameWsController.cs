using Casino.Services.Interfaces;
using Casino.Services.Models;
using Casino.Services.RequestResponse.BlackjackGame.Requests;
using Casino.Services.RequestResponse.BlackjackGame.Response;
using Casino.Web.WebSockets;
using Microsoft.AspNetCore.Mvc;

namespace Casino.Web.Controllers.WsControllers

{
    public class BlackJackGameWsController : WsController
    {
        private IBlackJackGameService _blackJackGameService;
        public BlackJackGameWsController(IBlackJackGameService blackJackGameService)
        {
            _blackJackGameService = blackJackGameService;
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
        public BaseResponse<BlackJackGameModel> TurnPlayer(TurnPlayerRequest gameId)
        {
            var response = _blackJackGameService.Turn(gameId.gameId, User.UserId);
            return response;
        }
        /// <summary>
        /// Пропуск хода игрока
        /// </summary>
        /// <param name="gameId">ИД игры</param>
        /// <returns> ид игры, статус игры, карты в руках игроков</returns>
        public BaseResponse<BlackJackGameModel> SkipPlayer(TurnPlayerRequest request)
        {
            var response = _blackJackGameService.SkipPlayer(request.gameId);
            return response;
        }
    }
}

