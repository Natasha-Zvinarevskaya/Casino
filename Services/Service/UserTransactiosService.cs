using Azure.Core;
using Casino.DataContext;
using Casino.DataContext.Enums;
using Casino.Services.Interfaces;
using Casino.Services.Models;
using Casino.Services.Models.UserTransactionService.Request;
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
        public UserTransactionService (DbContextOptions<CasinoDbContext> options)
        {
            _options = options;
        }
        public void EndGameTransaction(int gameId)
        {
            var db = new CasinoDbContext(_options);
            var game = db.PlayerGames.FirstOrDefault(x => x.Id == gameId);
            if (game == null)
                throw new Exception("Игра не найдена");
            var user = db.Users.FirstOrDefault(x => x.Id == game.UserId);
            if (user == null)
                throw new Exception("Пользователь не найдена");

            var gameSettings = db.GameSettings.FirstOrDefault(x => x.PlayerGameId == game.Id);
            if (gameSettings == null)
                throw new Exception("Настройки не найдена");

            var transactionBet = new UserTransactions()
            {
                Date = DateTime.UtcNow,
                PlayerGame = (int)game.Game,
                UsersId = user.Id
            };
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {

                    switch (game.Status)
                    {
                        case EnumStatusGame.Win:
                            user.Balance += game.AmountBet * gameSettings.AmountWin;
                            transactionBet.Type = EnumTypeTransaction.Win;
                            transactionBet.Amount = game.AmountBet * gameSettings.AmountWin;
                            break;
                        case EnumStatusGame.Loss:
                            user.Balance -= game.AmountBet;
                            transactionBet.Type = EnumTypeTransaction.Loss;
                            transactionBet.Amount = game.AmountBet;
                            break;

                    }
                    db.UserTransactions.Add(transactionBet);

                    db.SaveChanges();
                    transaction.Commit();

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                }

            }
        }

       /// <summary>
       /// Пополнение баланса
       /// </summary>
       /// <param name="userId">ид пользователя</param>
       /// <exception cref="Exception"></exception>
           public void ReplenishmentBalance(TopUpBalanceRequest request )
        {

            ////Console.Clear();
            ////Console.WriteLine("Выберите сумму пополнения баланса: \n1. 10 монет.\n2.30 монет.\n3.50 монет. ");
            ////var userAnswer = Console.ReadLine();
            var db = new CasinoDbContext(_options);
            var user = db.Users.FirstOrDefault(x => x.Id == request.UserId);
            if (user == null)
                throw new Exception("Пользователь не найден.");

            var transactionReplenishment = new UserTransactions()
            {
                Date = DateTime.UtcNow,
                Type = EnumTypeTransaction.Replenishment,
                UsersId = request.UserId,
                Amount = request.Count


            };
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    user.Balance += request.Count;

                    //    switch (userAnswer)
                    //    {
                    //        case "1":

                    //            transactionReplenisment.Amount = 10;
                    //            user.Balance += 10;
                    //            break;
                    //        case "2":
                    //            transactionReplenisment.Amount = 30;
                    //            user.Balance += 30;

                    //            break;
                    //        case "3":
                    //            transactionReplenisment.Amount = 50;
                    //            user.Balance += 50;

                    //            break;
                    //        default:
                    //            Console.WriteLine("Неверно выбрана сумма.");
                    //            break;


                //}
                        db.UserTransactions.Add(transactionReplenishment);
                db.SaveChanges();
                transaction.Commit();
            }
                    catch(Exception ex)
                    {
                transaction.Rollback();
            }
        }



    }
        
        
    }
}
