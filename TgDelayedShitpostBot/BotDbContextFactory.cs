using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace TgDelayedShitpostBot
{
    class BotDbContextFactory
    {
        public static BotDbContext Create(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<BotDbContext>();
            optionsBuilder.UseSqlite(connectionString);
            var context = new BotDbContext(optionsBuilder.Options);
            context.Database.EnsureCreated();
            return context;
        }
    }
}
