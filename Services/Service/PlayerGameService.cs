using Casino.DataContext;
using Casino.DataContext.Enums;
using Casino.Services.Interfaces;
using Casino.Services.Models;
using Casino.Services.RequestResponse.PlayerGameService.Request;
using Casino.Services.RequestResponse.PlayerGameService.Response;
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
        private IUserTransactionService _userService;
        private DbContextOptions<CasinoDbContext> _options;
        public PlayerGameService(IUserTransactionService userService, DbContextOptions<CasinoDbContext> options)
        {
            _userService = userService;
            _options = options;
        }

        //ToDo:InitGame,то же что и старт, только без пользователей,
        //ConnectPlayer(смотреть в настройках сколько игроков может играть),если игроков максимальное кол-во, то менять статус игры на Готово к игре),
        //DisconnectPlayers(наоборот),
        //GetGame (получить гейм Ид и игроков),
        //Сделать для игроков выбор комнаты со своим кол-вом макс игроков


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
                Games = (EnumGames)request.Game,
                GameSettings = new GameSettings() { AmountWin = 2, MaxCountPlayers = request.MaxCountPlayers },
                AmountBet = request.Bet,
                Status = EnumStatusGame.WaitingPlayers,
             
            };

            db.Games.Add(game);
            db.SaveChanges();

            ////Создаем список с UsersGame и добавляем его в текущую игру
            //var usersGames = new List<UsersGame>();
            //foreach (var userId in request.UserIds)
            //{
            //    usersGames.Add(new UsersGame { UserId = userId, GameId=game.Id });
            //}
            //game.UsersGames = usersGames;
            //db.SaveChanges();

            return game.Id;

        }
        /// <summary>
        /// Конец игры. Добавлем данные о времени окончания игры и результатов
        /// </summary>
        /// <param name="request">Результат игры, игра</param>
        public void EndGame(EndGameRequest request)
        {
            {
                var db = new CasinoDbContext(_options);
                var game = db.Games.FirstOrDefault(X => X.Id == request.GameId);
                if (game == null)
                    throw new Exception("Игра не найдена");
                game.DateEnd = DateTime.UtcNow;
                game.Status = request.ResultGame;
                db.SaveChanges();

                _userService.EndGameTransaction(game.Id, request.UserId);

            }
        }
        /// <summary>
        /// Запись в базу данных инфо о картах в руках и в колоде
        /// </summary>
        /// <param name="request">Колода, список со всеми картами игроков, список карт дилера и Ид игры</param>
        public void SaveGameHistory(SaveGameHistoryRequest request)
        {
            var db = new CasinoDbContext(_options);
            var historyModel = new CardsHistoryJson { Deck = request.Deck, Players = request.PlayersHands, PlayersSkiped = request.PlayersSkiped };
            string historyModelJson = JsonSerializer.Serialize(historyModel);
            var gameHistory = db.GameHistory.FirstOrDefault(x => x.GameId == request.GameId);
            if (gameHistory == null)
            {
                var saveGameHistory = new GameHistory { History = historyModelJson, GameId = request.GameId };
                db.GameHistory.Add(saveGameHistory);
                db.SaveChanges();
            }
            else
            {
                gameHistory.History = historyModelJson;

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

            ////Находим игру для того чтобы узнать статус игры
            //var userGame = db.UsersGames.FirstOrDefault(x => x.GameId == request.GameId);
            //if (userGame == null)
            //    throw new Exception("Игра не найдена.");
            var response = new GetHistoryResponse { CardsHistory = history, PlayersSkiped = history.PlayersSkiped }; //StatusGame = userGame.Game.Status };

            return response;
        }
        /// <summary>
        /// Подключение пользователя к игре
        /// </summary>
        /// <param name="userId">Ид пользователя</param>
        /// <param name="gameId">Ид игры</param>
        public BaseResponse ConnectPlayer(int userId, int gameId)
        {
            var db = new CasinoDbContext(_options);
            var userGame = db.UsersGames;
            userGame.Add(new UsersGame { GameId =  gameId , UserId = userId});
            var game = db.Games.FirstOrDefault(x => x.Id == gameId);
            var gameSettings = db.GameSettings.FirstOrDefault(x => x.GameId == gameId);
            //Сделать проверку на максимальное кол-во пользователей
            if (game.UsersGames.Count == gameSettings.MaxCountPlayers )
                game.Status = EnumStatusGame.ReadyToGame;
            db.SaveChanges();
            return new BaseResponse();
        }
        /// <summary>
        /// Отключение пользователя от игры 
        /// </summary>
        /// <param name="userId">Ид пользователя</param>
        /// <param name="gameId">Ид игры</param>
        public BaseResponse DisconnectPlayer(int userId, int gameId)
        {
            var db = new CasinoDbContext(_options);
            var userGame = db.UsersGames.FirstOrDefault(x => x.GameId == gameId && x.UserId == userId);
            if ( userGame == null)
            {
                throw new Exception("Пользовательская игра не найдена.");
            }
            var game = db.Games.FirstOrDefault(x=>x.Id == gameId);
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
        public void CreateGame(BaseUserIdReq<StartGameRequest> request)
        {
            var gameId = StartGame(request.Request);
            ConnectPlayer(request.UserId, gameId);
        }
        /// <summary>
        /// Проверка все ли пользователи подключились к игре
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns></returns>
        public bool IsGameReady (int gameId)
        {
            var db = new CasinoDbContext(_options);
            var game = db.Games.FirstOrDefault(x=>x.Id == gameId);
            if (game.Status == EnumStatusGame.ReadyToGame)
                return true;
            return false;
        }


    }
}
