using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Xml;

namespace Casino.DataContext
{
    public class CasinoDbContext : DbContext
    {
        public CasinoDbContext(DbContextOptions<CasinoDbContext> options) : base(options)
        {

        }
       
        private const string connectionString = "Server=localhost\\SQLEXPRESS;Database=Casino;Trusted_Connection=True;TrustServerCertificate=True";

        public DbSet<Users> Users { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<GameSettings> GameSettings { get; set; }
        public DbSet<UserTransactions> UserTransactions { get; set; }
        public DbSet<UserSession> UserSessions { get; set; }
        public DbSet<GameHistory> GameHistory { get; set; }
        public DbSet<UserProvider> UserProviders { get; set; }

        public DbSet<StripeCustomer> StripeCustomers { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<MoneyTransaction> MoneyTransactions { get; set; }
        public DbSet<UsersGame> UsersGames { get; set; }

        public DbSet<DealerBJSettings> DealerBJSettings { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<PlayerGame>().HasOne(x => x.User).WithMany(x => x.PlayerGames);
            modelBuilder.Entity<UserTransactions>().HasOne(x => x.User).WithMany(x => x.Transactions).HasForeignKey(x => x.UsersId);
            modelBuilder.Entity<Game>().HasOne(x => x.GameSettings).WithOne(x => x.Game).HasForeignKey<GameSettings>(x => x.GameId);
            modelBuilder.Entity<UserSession>().HasOne(x => x.User).WithMany(x => x.UserSessions).HasForeignKey(x => x.UserId);
            modelBuilder.Entity<Game>().HasOne(x => x.GameHistory).WithOne(x => x.Game).HasForeignKey<GameHistory>(x => x.GameId);
            modelBuilder.Entity<UserProvider>().HasOne(x => x.User).WithOne(x => x.UserProvider).HasForeignKey<UserProvider>(x => x.UserId);

            modelBuilder.Entity<MoneyTransaction>().HasOne(x => x.User).WithMany(x => x.MoneyTransactions).HasForeignKey(x => x.UserId);
            modelBuilder.Entity<StripeCustomer>().HasOne(x => x.User).WithOne(x => x.StripeUser).HasForeignKey<StripeCustomer>(x => x.UserId);
            modelBuilder.Entity<Payment>().HasOne(x => x.User).WithMany(x => x.Payments).HasForeignKey(x => x.UserId);

            modelBuilder.Entity<UsersGame>().HasKey(x => new { x.UserId, x.GameId });
            
            modelBuilder.Entity<UsersGame>().HasOne(x => x.Game).WithMany(x => x.UsersGames).HasForeignKey(x => x.GameId);
            modelBuilder.Entity<UsersGame>().HasOne(x => x.User).WithMany(x => x.UsersGames).HasForeignKey(x => x.UserId);

            modelBuilder.Entity<DealerBJSettings>()
          .Property(x => x.Id)
          .ValueGeneratedNever(); // Отключает автогенерацию


        }

    }
}
