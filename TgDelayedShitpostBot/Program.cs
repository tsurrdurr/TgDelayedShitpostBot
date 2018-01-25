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
                
            }
            catch
            {
                GarbageFunctionality.Log("Shutting down");
            }
        }
    }
}
