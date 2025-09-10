using Casino.DataContext;
using Casino.DataContext.Enums;
using Casino.Services.Interfaces;
using Casino.Services.Models;
using Casino.Services.Models.PlayerGameService.Request;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Casino.Services.Service
{
    public class PlayerGameService : IPlayerGameService
    {
        private IUserTransactionService _userService;
        private DbContextOptions< CasinoDbContext> _options;
        public PlayerGameService(IUserTransactionService userService,DbContextOptions <CasinoDbContext> options)
        {
            _userService = userService;
            _options = options;
        }
        /// <summary>
        /// Запуск игры
        /// </summary>
        /// <param name="request">Id пользователя, выбранная игра</param>
        /// <returns>Игра</returns>
        public int StartGame(StartGameRequest request)
        {
            var db = new CasinoDbContext(_options);
            var user = db.Users.FirstOrDefault(x => x.Id == request.UserId);
            if (user == null)
                throw new Exception("Пользователь не найден.");
            //if (user.Balance<10)
            //{
            //    _userService.ReplenishmentBalance(user.Id);
            //}
            
                var game = new PlayerGame()
                {
                    Date = DateTime.UtcNow,
                    Game = (EnumGames)request.Game,
                    GameSettings = new GameSettings() { AmountWin = 2 },
                    AmountBet = request.Bet,
                    Status = EnumStatusGame.None,
                    UserId = request.UserId

                };
            db.PlayerGames.Add(game);
            db.SaveChanges();
       
            return game.Id;

        }
        /// <summary>
        /// Конец игры
        /// </summary>
        /// <param name="request">Результат игры, игра</param>
        public void EndGame(EndGameRequest request)
        {
            {
                var db = new CasinoDbContext(_options);
                var game = db.PlayerGames.FirstOrDefault(X => X.Id == request.GameId);
                if (game == null)
                    throw new Exception("Игра не найдена");
                game.DateEnd = DateTime.UtcNow;
                game.Status = request.ResultGame;
                db.SaveChanges();

                _userService.EndGameTransaction(game.Id);

            }
        }
        /// <summary>
        /// Запись в базу данных инфо о картах в руках и в колоде
        /// </summary>
        /// <param name="request"></param>
        public void SaveGameHistory(SaveGameHistoryRequest request  )
        {
            var db = new CasinoDbContext(_options);
            var historyModel = new CardsHistoryJson { Deck = request.Deck, PlayerHand = request.PlayerHand, DealerHand = request.DealerHand };
            string historyModelJson = JsonSerializer.Serialize(historyModel);
            var gameHistory = new GameHistory { CardsHistory = historyModelJson, PlayerGameId = request.GameId };
            db.GameHistory.Add(gameHistory);
            db.SaveChanges();
        }



    }
}
