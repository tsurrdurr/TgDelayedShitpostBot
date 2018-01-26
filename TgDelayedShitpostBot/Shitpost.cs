using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TgDelayedShitpostBot
{
    public class Shitpost
    {
        public Shitpost(Telegram.Bot.Types.Message msg)
        {
            this.senderId = msg.From.Id;
            this.chatId = msg.Chat.Id;
            this.messageId = msg.MessageId;
            this.addedTime = DateTime.Now;
        }

        public Shitpost() { }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int shitpostId { get; set; }
        public int senderId { get; set; }
        public long chatId { get; set; }
        public int messageId { get; set; }
        public DateTime addedTime { get; set; }
        public DateTime postDueTime { get; set; }
    }
}
