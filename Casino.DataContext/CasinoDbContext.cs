using Microsoft.EntityFrameworkCore;
using System;

namespace Casino.DataContext
{
    public class CasinoDbContext : DbContext
    {
        public CasinoDbContext(DbContextOptions<CasinoDbContext> options) : base(options)
        {

        }
        //public CasinoDbContext()
        //{
        //    Database.EnsureCreated();
        //}
        private const string connectionString = "Server=localhost\\SQLEXPRESS;Database=Casino;Trusted_Connection=True;TrustServerCertificate=True";

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(connectionString);
        //}
        public DbSet<Users> Users { get; set; }
        public DbSet<PlayerGame> PlayerGames {get;set;}
        public DbSet<GameSettings> GameSettings { get; set; }
        public DbSet<UserTransactions> UserTransactions { get; set; }
        public DbSet<UserSession> UserSessions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PlayerGame>().HasOne(x => x.User).WithMany(x => x.PlayerGames);
            modelBuilder.Entity<UserTransactions>().HasOne(x => x.User).WithMany(x => x.Transactions).HasForeignKey(x => x.UsersId);
            modelBuilder.Entity<PlayerGame>().HasOne(x => x.GameSettings).WithOne(x => x.PlayerGame).HasForeignKey<GameSettings>(x => x.PlayerGameId);
            modelBuilder.Entity<UserSession>().HasOne(x => x.User).WithMany(x => x.UserSessions).HasForeignKey(x => x.UserId);

        }

    }
}
