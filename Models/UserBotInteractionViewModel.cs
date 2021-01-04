using System;

namespace Algoromida_01.Models
{
    public class UserBotInteractionViewModel
    {
        public string BotId { get; set; }
        public UserBotInteraction[] Interactions { get; set; }

        public string GetLastBotInteraction()
        {
            
            if (Interactions.Length > 0)
            {
                return Interactions[Interactions.Length - 1].BotResponse;
            } else
            {
                return "You have no message history with " + BotId + " yet.";
            }
        }

        public string GetLastBotInteractionTime()
        {
            if (Interactions.Length > 0)
            {
                return Interactions[Interactions.Length - 1].CompletedAt.ToString();
            }
            else
            {
                return DateTimeOffset.UtcNow.ToString();
            }
        }
    }
}
