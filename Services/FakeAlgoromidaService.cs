using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Algoromida_01.Models;

namespace Algoromida_01.Services
{
    public class FakeAlgoromidaService : IAlgoromidaService
    {
        public Task<UserBotInteraction[]> GetPreviousAsync(string userid, string botid)
        {
            var interaction1 = new UserBotInteraction
            {
                UserId = userid,
                BotId = botid,
                UserQuery = "Hi Dona",
                InitiatedAt = DateTimeOffset.Now,
                BotResponse = "Hi San",
                BotAwareness = "g",
                BotStatefulness = "0",
                CompletedAt = DateTimeOffset.Now,
                IsComplete = true

            };
            var interaction2 = new UserBotInteraction
            {
                UserId = userid,
                BotId = botid,
                UserQuery = "How are you doing?",
                InitiatedAt = DateTimeOffset.Now,
                BotResponse = "I am doing alright.",
                BotAwareness = "s",
                BotStatefulness = "0",
                CompletedAt = DateTimeOffset.Now,
                IsComplete = true
            };
            return Task.FromResult(new[] { interaction1, interaction2 });
        }

        public async Task<UserBotInteraction> RespondAsync(string userid, string botid, string query)
        {
            var interaction1 = new UserBotInteraction
            {
                UserId = userid,
                BotId = botid,
                UserQuery = query,
                InitiatedAt = DateTimeOffset.Now,
                BotResponse = "generic response",
                BotAwareness = "s",
                BotStatefulness = "0",
                CompletedAt = DateTimeOffset.Now,
                IsComplete = true

            };
            return interaction1;
        }

    }
}
