using Casino.DataContext;
using Casino.DataContext.Enums;
using Casino.DataContext.Models.BlackjackGame;
using Casino.Services.Interfaces;
using Casino.Services.Models;
using Casino.Services.RequestResponse.PlayerGameService.Request;
using Casino.Services.RequestResponse.PlayerGameService.Response;
using Casino.Services.RequestResponse.UserTransactionService.Request;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
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
        private IUserTransactionService _userTransactionService;
        private DbContextOptions<CasinoDbContext> _options;
        private IUserService _userService;
        public PlayerGameService(IUserTransactionService userTransactionService, DbContextOptions<CasinoDbContext> options, IUserService userService)
        {
            _userTransactionService = userTransactionService;
            _options = options;
            _userService = userService;
        }
        /// <summary>
        /// Запуск игры
        /// </summary>
        /// <param name="request"> выбранная игра и сумма ставки</param>
        /// <returns>Игра</returns>
        public int StartGame(StartGameRequest request)
        {
            var db = new CasinoDbContext(_options);

            //Создаем игру и записываем в бд
            var game = new Game()
            {
                Date = DateTime.UtcNow,
                Games = request.Game,
                GameSettings = new GameSettings() { AmountWin = 2, MaxCountPlayers = request.MaxCountPlayers },
                AmountBet = request.Bet,
                Status = EnumStatusGame.WaitingPlayers,

            };

            db.Games.Add(game);
            db.SaveChanges();
            return game.Id;
        }
        /// <summary>
        /// Конец игры. Добавлем данные о времени окончания игры и результатов
        /// </summary>
        /// <param name="request">Результат игры, игра</param>
        public BaseResponse EndGame(EndGameRequest request)
        {
            {
                var db = new CasinoDbContext(_options);
                var game = db.Games.FirstOrDefault(X => X.Id == request.GameId);
                if (game == null)
                    throw new Exception("Игра не найдена");
                game.DateEnd = DateTime.UtcNow;
                game.Status = request.ResultGame;
                db.SaveChanges();

                foreach (var player in request.Players)
                {
                    _userTransactionService.EndGameTransaction(new EndGameTransactionRequest() { UserId = player.UserId, GameId = game.Id, StatusGame = player.StatusGame });

                }
                return new BaseResponse();
            }
        }
        /// <summary>
        /// Запись в базу данных инфо о картах в руках и в колоде
        /// </summary>
        /// <param name="request">Колода, список со всеми картами игроков, список карт дилера и Ид игры</param>
        public void SaveGameHistory(SaveGameHistoryRequest request)
        {
            var db = new CasinoDbContext(_options);
            var historyModel = new CardsHistoryJson { Deck = request.Deck, Players = request.PlayersHands };
            string historyModelJson = JsonSerializer.Serialize(historyModel);
            var gameHistory = db.GameHistory.FirstOrDefault(x => x.GameId == request.GameId);
            if (gameHistory == null)
            {
                var saveGameHistory = new GameHistory
                {
                    History = historyModelJson,
                    GameId = request.GameId,
                    Cheating = request.Cheating,
                    Risk = request.Risk,
                    WinCheating = request.WinCheating,
                    IsCrook = request.IsCrook
                };
                db.GameHistory.Add(saveGameHistory);
                db.SaveChanges();
            }
            else
            {
                gameHistory.History = historyModelJson;
                gameHistory.Risk = request.Risk;
                gameHistory.Cheating = request.Cheating;
                gameHistory.WinCheating = request.WinCheating;
                gameHistory.IsCrook = request.IsCrook;

            }
            db.SaveChanges();
        }
        /// <summary>
        /// Вернуть историю игры
        /// </summary>
        /// <param name="gameId">ИД игры</param>
        /// <returns>Историю карт(колода, карты игрока, карты дилера) и статус игры</returns>
        /// <exception cref="Exception"></exception>
        public GetHistoryResponse GetHistory(GetHistoryRequest request)
        {

            //Находим историю игры 
            var db = new CasinoDbContext(_options);
            var gameHistory = db.GameHistory.FirstOrDefault(x => x.GameId == request.GameId);
            if (gameHistory == null)
                throw new Exception("Игра не найдена");

            //Десериализуем историю 
            var history = JsonSerializer.Deserialize<CardsHistoryJson>(gameHistory.History);
            var response = new GetHistoryResponse
            {
                CardsHistory = history,
                PlayersSkiped = history.PlayersSkiped,
                Cheating = gameHistory.Cheating,
                Risk = gameHistory.Risk,
                WinCheating = gameHistory.WinCheating,
                IsCrook = gameHistory.IsCrook
            };
            return response;
        }
        /// <summary>
        /// Подключение пользователя к игре
        /// </summary>
        /// <param name="userId">Ид пользователя</param>
        /// <param name="gameId">Ид игры</param>
        public BaseResponse<EnumStatusGame> ConnectPlayer(BaseUserIdReq<ConnectPlayerRequest> request)
        {
            var db = new CasinoDbContext(_options);
            var user = db.UsersGames.FirstOrDefault(x => x.UserId == request.UserId && x.GameId == request.Request.GameId);
            if (user != null)
                throw new Exception("Пользователь уже подключен к игре.");
            //Проверка баланса игрока
            var userData = _userService.GetUserData(request.UserId);
            if (userData.Data.Balance <= request.Request.Bet)
                throw new Exception("Недостаточно средств,чтобы начать игру.");

            db.UsersGames.Add(new UsersGame { GameId = request.Request.GameId, UserId = request.UserId });
            var game = db.Games.FirstOrDefault(x => x.Id == request.Request.GameId);
            if (game == null)
                throw new Exception("Игра не найдена.");
            db.SaveChanges();

            var countUsers = db.UsersGames.Where(x => x.GameId == request.Request.GameId).Select(x => x.UserId).ToList();

            var gameSettings = db.GameSettings.FirstOrDefault(x => x.GameId == request.Request.GameId);
            //Сделать проверку на максимальное кол-во пользователей
            if (countUsers.Count == gameSettings.MaxCountPlayers)
            {
                game.Status = EnumStatusGame.ReadyToGame;
                db.SaveChanges();
                return new BaseResponse<EnumStatusGame>(EnumStatusGame.ReadyToGame);
            }
            return new BaseResponse<EnumStatusGame>(EnumStatusGame.WaitingPlayers);
        }
        /// <summary>
        /// Отключение пользователя от игры 
        /// </summary>
        /// <param name="userId">Ид пользователя</param>
        /// <param name="gameId">Ид игры</param>
        public BaseResponse DisconnectPlayer(BaseUserIdReq<int> request)
        {
            var db = new CasinoDbContext(_options);
            var userGame = db.UsersGames.FirstOrDefault(x => x.GameId == request.Request && x.UserId == request.UserId);
            if (userGame == null)
            {
                throw new Exception("Пользовательская игра не найдена.");
            }
            var game = db.Games.FirstOrDefault(x => x.Id == request.Request);
            if (game.Status == EnumStatusGame.ReadyToGame)
                throw new Exception("Невозможно выйти при активной игре.");
            else
            {
                db.UsersGames.Remove(userGame);
                db.SaveChanges();
            }
            return new BaseResponse();
        }

        /// <summary>
        /// Создание новой игры
        /// </summary>
        /// <param name="request">Ид пользователя, выбранная игра и сумма ставки </param>
        public CreateGameResponce CreateGame(BaseUserIdReq<StartGameRequest> request)
        {
            var gameId = StartGame(request.Request);
            // ConnectPlayer(new BaseUserIdReq<int>(request.UserId, gameId));
            return (new CreateGameResponce { GameId = gameId });
        }

        /// <summary>
        /// Проверка все ли пользователи подключились к игре
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns></returns>
        public EnumStatusGame IsGameReady(IsGameReadyRequest req)
        {
            var db = new CasinoDbContext(_options);
            var game = db.Games.FirstOrDefault(x => x.Id == req.GameId);
            return game.Status;
        }


    }
}
