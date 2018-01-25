using System;

namespace TgDelayedShitpostBot
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var settings = Settings.Instance();
                var api = new TelegramAPIClass(settings.token);
            }
            catch(Exception ex)
            {
                GarbageFunctionality.Log("Shutting down");
            }
        }
    }
}
