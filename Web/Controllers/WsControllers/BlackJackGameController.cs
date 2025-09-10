using Casino.Services.Interfaces;
using Casino.Services.Models.BlackjackGame.Requests;
using Casino.Services.Models.BlackjackGame.Response;
using Casino.Web.WebSockets;
using Microsoft.AspNetCore.Mvc;

namespace Casino.Web.Controllers.WsControllers

{    public class BlackJackGameController : WsController
    {
        private IBlackJackGameService _blackJackGameService;
        public BlackJackGameController(IBlackJackGameService blackJackGameService)
        {
            _blackJackGameService = blackJackGameService;
        }

       
        /// <summary>
        /// Начало игры 
        /// </summary>
        /// <param name="request">Ставка пользователя</param>
        /// <returns></returns>
        public BaseResponse<BlackJackGameModel> StartGame(BlackjackPlayRequest request)
        {
            var response = _blackJackGameService.Play(request);
            return response;
        }
        /// <summary>
        /// Ход игрока
        /// </summary>
        /// <param name="">Ид игры</param>
        /// <returns></returns>
      //  public BaseResponse<BlackJackGameModel> TurnPlayer(GameId)
        //{

        //}
        //public BaseResponse<BlackJackGameModel> SkipPlayer(gameId)
        //{

        //}
    }
}

