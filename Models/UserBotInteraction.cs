using System;
using System.ComponentModel.DataAnnotations;

namespace Algoromida_01.Models
{
    public class UserBotInteraction
    {
        [Key]
        public Guid Id { get; set; }

        public string UserId { get; set; }

        public string BotId { get; set; }

        [Required]
        public string UserQuery { get; set; }

        public DateTimeOffset? InitiatedAt { get; set; }

        public string BotResponse { get; set; }

        public string BotAwareness { get; set; }

        public string BotStatefulness { get; set; }

        public DateTimeOffset? CompletedAt { get; set; }

        public bool IsComplete { get; set; }

        public bool UserLikedIt { get; }
    }
}
