using System;
using System.ComponentModel.DataAnnotations;

// Here, we want to add UserPostback, BotOptions, ResponseImages and think of how this can be represented
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

        public bool IsSuccessful { get; set; }

        public bool UserLikedIt { get; set; }

        public string BotAvatarPath { get; set; }

        public string UserAvatarPath { get; set; }

        public UserBotInteraction()
        {
            Id = Guid.NewGuid();
            InitiatedAt = DateTimeOffset.Now;
            UserLikedIt = false;
        }
    }
}
