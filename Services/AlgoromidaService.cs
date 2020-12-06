using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;

using Algoromida_01.Models;

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

            //Console.WriteLine("query from service is: " + userBotInteraction.UserQuery);
            //Console.WriteLine("Making API Call...");

            var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate });
            client.BaseAddress = new Uri("https://08ddba9b69fc.ngrok.io/");
            HttpResponseMessage response = client.GetAsync("predict?text=" + userBotInteraction.UserQuery).Result;
            var successful = response.IsSuccessStatusCode;
            if (!successful)
            {
                return false;
            }

            string result = response.Content.ReadAsStringAsync().Result;

            userBotInteraction.BotAwareness = "s";
            userBotInteraction.BotStatefulness = "0";
            userBotInteraction.BotResponse = result;
            userBotInteraction.CompletedAt = DateTimeOffset.Now;
            userBotInteraction.IsComplete = true;

            
            _algoromidaContext.Add(userBotInteraction);
            var saveResult = await _algoromidaContext.SaveChangesAsync();

            return saveResult == 1;
        }
    }
}
