using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;

namespace TgDelayedShitpostBot
{
    public class TelegramAPIClass
    {
        private static TelegramBotClient Bot;


        public TelegramAPIClass(string token)
        {
            Bot = new TelegramBotClient(token);
            var me = Bot.GetMeAsync().Result;
            Console.Title = "Shitpost machine";
            BindBotEvents();
            Bot.StartReceiving();
            Console.WriteLine($"Shitposting activated on @{me.Username}");
            Console.ReadLine();
            Bot.StopReceiving();
        }

        private void BindBotEvents()
        {
            Bot.OnMessage += Bot_OnMessage;
        }

        private void Bot_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            var message = e.Message;
            if (SenderCanAddPosts(message))
            {
                if (e.Message.Photo != null)
                {
                    AddToRepostQueue(message);
                }
                else
                {
                    HandleNonPicture(message);
                }
            }
            else
            {
                Bot.SendTextMessageAsync(message.Chat, "You are not my master!");
            }
        }

        private static void HandleNonPicture(Telegram.Bot.Types.Message message)
        {
            if (message.Text == "/help")
            {
                Bot.SendTextMessageAsync(message.Chat, "Nothing here");
            }
            else
            {
                Bot.SendTextMessageAsync(message.Chat, @"Send me a picture. Use /help");
            }
        }

        private void AddToRepostQueue(Telegram.Bot.Types.Message msg)
        {
            using (var context = BotDbContextFactory.Create(Settings.Instance().connectionString))
            {
                var shitpost = new Shitpost(msg);
                context.Add(shitpost);
                context.SaveChanges();
            }
            Bot.ForwardMessageAsync(Settings.Instance().chatId, msg.Chat.Id, msg.MessageId);
        }

        private bool SenderCanAddPosts(Telegram.Bot.Types.Message message)
        {
            return message.From.Id == Settings.Instance().ownerId;
        }
    }
}
