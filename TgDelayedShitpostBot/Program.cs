using System;
using System.Linq;

namespace TgDelayedShitpostBot
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var settings = Settings.Instance();
                using (var context = BotDbContextFactory.Create(Settings.Instance().connectionString))
                {
                    var listFromDb = (from all in context.Shitposts select all).ToList();
                    ShitpostQueue.posts = listFromDb;
                }
                
                var api = new TelegramAPIClass(settings.token);
            }
            catch(Exception ex)
            {
                GarbageFunctionality.Log("Shutting down");
            }
        }
    }
}
