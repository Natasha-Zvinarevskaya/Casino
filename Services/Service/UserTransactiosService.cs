using Azure.Core;
using Casino.DataContext;
using Casino.DataContext.Enums;
using Casino.Services.Interfaces;
using Casino.Services.Models;
using Casino.Services.RequestResponse.UserTransactionService.Request;
using Casino.Services.RequestResponse.UserTransactionService.Response;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Services.Service
{
    public class UserTransactionService : IUserTransactionService
    {
        private DbContextOptions<CasinoDbContext> _options;
        public UserTransactionService(DbContextOptions<CasinoDbContext> options)
        {
            _options = options;
        }
        /// <summary>
        /// Транзакция при окончании игры
        /// </summary>
        /// <param name="gameId">Ид игры</param>
        /// <exception cref="Exception"></exception>
        public BaseResponse EndGameTransaction(EndGameTransactionRequest request)
        {
            //Находим сыгранную игру и пользователя
            var db = new CasinoDbContext(_options);
            var userGame = db.UsersGames.FirstOrDefault(x=>x.GameId == request.GameId && x.UserId == request.UserId);
            if (userGame == null)
                throw new Exception("Пользовательская игра не найдена.");
            
            var game = db.Games.FirstOrDefault(x => x.Id == request.GameId );
            
            var user = db.Users.FirstOrDefault(x => x.Id == request.UserId);
            

            //Находим настройки для определения коэффициента выигрыша/проигрыша
            var gameSettings = db.GameSettings.FirstOrDefault(x => x.GameId == game.Id);
            if (gameSettings == null)
                throw new Exception("Настройки не найдены");

            //Создаем транзакцию
            var transactionBet = new UserTransactions()
            {
                Date = DateTime.UtcNow,
                Game = game.Games,
                UsersId = user.Id
            };
            //Используем специальные команды,чтобы не было никаких ошибок при оплате
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    switch (request.StatusGame)
                    {
                        case EnumStatusPlayerGame.Win:
                            user.Balance += game.AmountBet * gameSettings.AmountWin;
                            transactionBet.Type = EnumTypeTransaction.Win;
                            transactionBet.Amount = game.AmountBet * gameSettings.AmountWin;
                            break;
                        case EnumStatusPlayerGame.Loss:
                            user.Balance -= game.AmountBet;
                            transactionBet.Type = EnumTypeTransaction.Loss;
                            transactionBet.Amount = game.AmountBet;
                            break;
                    }
                    db.UserTransactions.Add(transactionBet);

                    db.SaveChanges();
                    transaction.Commit();
                    return new BaseResponse();

                }
                //При ошибке оплаты возвращает деньги 
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return new BaseResponse("Ошибка транзакции.");
                }

            }
        }

        /// <summary>
        /// Пополнение баланса
        /// </summary>
        /// <param name="userId">ид пользователя</param>
        /// <exception cref="Exception"></exception>
        public BaseResponse ReplenishmentBalance(BaseUserIdReq<TopUpBalanceRequest> request)
        {
            var db = new CasinoDbContext(_options);
            var user = db.Users.FirstOrDefault(x => x.Id == request.UserId);
            if (user == null)
                throw new Exception("Пользователь не найден.");

            var transactionReplenishment = new UserTransactions()
            {
                Date = DateTime.UtcNow,
                Type = EnumTypeTransaction.Replenishment,
                UsersId = request.UserId,
                Amount = request.Request.Count
            };
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    user.Balance += request.Request.Count;
                    db.UserTransactions.Add(transactionReplenishment);
                    db.SaveChanges();
                    transaction.Commit();
                    return new BaseResponse();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return new BaseResponse("Ошибка транзакции.");
                }
            }
        }
        /// <summary>
        /// Метод получения истории пользователя: кол-во сыгранных игр, победы/проигрыши/ничьи, кол-во денег
        /// </summary>
        /// <param name="userId">Ид пользователя</param>
        public BaseResponse<GetHistoryTransactionResponse> GetHistoryTransactions(GetHistoryTransactionsRequest request)
        {
            var db = new CasinoDbContext(_options);
            var userTransactions = db.UserTransactions.Where(x => x.UsersId == request.UserId).ToList();
            if (userTransactions == null)
                throw new Exception("Пользователь не найден.");

            var countWins = 0;
            var countLosses = 0;
            var countDraws = 0;
            var countGames = 0;
            decimal amountLost = 0;
            decimal amountWon = 0;


            foreach (var userTransaction in userTransactions)
            {
                if (userTransaction.Type == EnumTypeTransaction.Win)
                {
                    countWins += 1;
                    countGames += 1;
                    amountWon += userTransaction.Amount;
                }
                if (userTransaction.Type == EnumTypeTransaction.Loss)
                {
                    countLosses += 1;
                    countGames += 1;
                    amountLost += userTransaction.Amount;
                }
                if (userTransaction.Type == EnumTypeTransaction.Draw)
                {
                    countDraws += 1;
                    countGames += 1;
                }

            }
            var response = new GetHistoryTransactionResponse
            {
                CountWins = countWins,
                CountLosses = countLosses,
                CountDraws = countDraws,
                CountGames = countGames,
                AmountLost = amountLost,
                AmountWon = amountWon
            };
            return new BaseResponse<GetHistoryTransactionResponse>(response);
        }


    }
}
