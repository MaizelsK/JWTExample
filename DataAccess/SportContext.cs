using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess
{
    public class SportContext : DbContext
    {
        public SportContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<Player> Players { get; set; }
        public DbSet<Team> Team { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var team = new Team
            {
                Id = 1,
                Name = "OG"
            };

            var player = new Player
            {
                Id = 1,
                FullName = "Bobby",
                Number = 10,
                TeamId = 1
            };

            modelBuilder.Entity<Team>().HasData(team);
            modelBuilder.Entity<Player>().HasData(player);

            base.OnModelCreating(modelBuilder);
        }
    }
}
