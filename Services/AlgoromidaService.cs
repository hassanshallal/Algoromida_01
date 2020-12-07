using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
//using Json.Net;

using Algoromida_01.Models;
using System.Text;

namespace Algoromida_01.Services
{
    public class AlgoromidaService : IAlgoromidaService
    {
        private readonly AlgoromidaContext _algoromidaContext;
        public AlgoromidaService(AlgoromidaContext algoromidaContext)
        {
            _algoromidaContext = algoromidaContext;
        }

        public async Task<UserBotInteraction[]> GetPreviousAsync(AlgoromidaUser user)
        {
            return await _algoromidaContext.Interactions.Where(x => x.UserId == user.Id).ToArrayAsync();
        }

        public async Task<bool> RespondAsync(UserBotInteraction userBotInteraction, AlgoromidaUser user)
        {
            userBotInteraction.Id = Guid.NewGuid();
            userBotInteraction.UserId = user.Id;
            userBotInteraction.BotId = "Dona";
            userBotInteraction.InitiatedAt = DateTimeOffset.Now;

            var donaPrimer = new Dictionary<string, object>
            {
                ["text"] = userBotInteraction.UserQuery,
                ["senderInfo"] = ("(" + user.Id + ", ") + (user.FirstName + ", ") + (user.LastName + ", ") + (user.Location + ", ") + (user.Gender + ", ") + ("Algoromida)")
            };
            var payload = JsonSerializer.Serialize(donaPrimer);
            
            
            var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate });
            client.BaseAddress = new Uri("https://08ddba9b69fc.ngrok.io/predict?");
            HttpContent body = new StringContent(payload, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(client.BaseAddress, body).Result;

            var successful = response.IsSuccessStatusCode;
            if (!successful)
            {
                return false;
            }

            string result = response.Content.ReadAsStringAsync().Result;
            var results = JsonSerializer.Deserialize<Dictionary<string, string>>(result);

            userBotInteraction.BotResponse = results["BotResponse"];
            userBotInteraction.BotAwareness = results["BotAwareness"];
            userBotInteraction.BotStatefulness = results["BotStatefulness"];
            userBotInteraction.CompletedAt = DateTimeOffset.Now;
            userBotInteraction.IsComplete = true;

            _algoromidaContext.Add(userBotInteraction);
            var saveResult = await _algoromidaContext.SaveChangesAsync();

            return saveResult == 1;
        }
    }
}
