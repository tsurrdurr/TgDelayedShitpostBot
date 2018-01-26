using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace TgDelayedShitpostBot
{
    class BotDbContext : DbContext
    {
        public BotDbContext(DbContextOptions<BotDbContext> options) : base(options) { }

        public DbSet<Shitpost> Shitposts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Shitpost>().ToTable("Shitpost");
        }
    }
}
