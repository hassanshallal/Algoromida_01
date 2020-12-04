using System;
using System.ComponentModel.DataAnnotations;

namespace Algoromida_01.Models
{
    public class UserBotInteraction
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public string BotId { get; set; }

        [Required]
        public string UserQuery { get; set; }

        [Required]
        public DateTimeOffset? InitiatedAt { get; set; }

        [Required]
        public string BotResponse { get; set; }

        [Required]
        public string BotAwareness { get; set; }

        [Required]
        public string BotStatefulness { get; set; }

        [Required]
        public DateTimeOffset? CompletedAt { get; set; }

        [Required]
        public bool IsComplete { get; set; }
    }
}
