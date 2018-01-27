using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using Telegram.Bot;

namespace TgDelayedShitpostBot
{
    public class TelegramAPIClass
    {
        private static TelegramBotClient Bot;
        private bool canAddAnythingToQueue = false;

        private Timer timer = new Timer
        {
            Interval = Settings.Instance().timerIntervalSeconds * 1000,
            Enabled = false
        };

        public TelegramAPIClass(string token)
        {
            Bot = new TelegramBotClient(token);
            timer.Elapsed += Timer_Elapsed;
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
            if(ShitpostQueue.posts.Count > 0)
            {
                timer.Enabled = true;
            }
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var shitpost = ShitpostQueue.posts[0];
            Bot.ForwardMessageAsync(Settings.Instance().chatId, shitpost.senderId, shitpost.messageId);
            using (var context = BotDbContextFactory.Create(Settings.Instance().connectionString))
            {
                context.Shitposts.Remove(shitpost);
                context.SaveChanges();
            }
            ShitpostQueue.posts.RemoveAt(0);
            if (ShitpostQueue.posts.Count == 0) timer.Enabled = false;
        }

        private async void Bot_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            var message = e.Message;
            if(message.Chat.Type == Telegram.Bot.Types.Enums.ChatType.Private)
            {
                if (SenderCanAddPosts(message))
                {
                    if (e.Message.Photo != null)
                    {
                        AddToRepostQueue(message);
                    }
                    else if(message.Text == @"/queue")
                    {
                        canAddAnythingToQueue = true;
                        Bot.SendTextMessageAsync(message.Chat, "Awaiting for a message to queue.");
                    }
                    else if (canAddAnythingToQueue)
                    {
                        canAddAnythingToQueue = false;
                        AddToRepostQueue(message);
                    }
                    else
                    {
                        HandleNonPicture(message);
                    }
                }
                else
                {
                    if (!message.From.IsBot)
                        Bot.SendTextMessageAsync(message.Chat, "You are not my master!");
                }
            }
        }

        private static void HandleNonPicture(Telegram.Bot.Types.Message message)
        {
            if (message.Text == "/help")
            {
                Bot.SendTextMessageAsync(message.Chat, "Post pictures you want to add to repost queue.\nUse /queue if you want to repost a non-picture in next message");
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
                ShitpostQueue.posts.Add(shitpost);
                context.Add(shitpost);
                context.SaveChanges();
            }
            Bot.SendTextMessageAsync(msg.Chat, "Message queued");
            if (!timer.Enabled) timer.Enabled = true;
        }

        private bool SenderCanAddPosts(Telegram.Bot.Types.Message message)
        {
            return message.From.Id == Settings.Instance().ownerId;
        }
    }
}
