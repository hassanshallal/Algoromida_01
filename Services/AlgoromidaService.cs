using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using System.Net;
using System.Net.Http;

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


        public async Task<UserBotInteraction[]> GetPreviousAsync(string userid, string botid)
        {
            return await _algoromidaContext.Interactions.Where(x => x.UserId == userid & x.BotId == botid).ToArrayAsync();
        }

        public async Task<UserBotInteraction> RespondAsync(string userid, string botid, string query)
        {
            var iniatedAt = DateTimeOffset.Now;

            Console.WriteLine("Making API Call...");
            var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate });
            client.BaseAddress = new Uri("https://08ddba9b69fc.ngrok.io/");
            HttpResponseMessage response = client.GetAsync("predict?text=" + query).Result;
            var successful = response.IsSuccessStatusCode;
            if (!successful)
            {
                //return BadRequest("Could not get Dona response.");
                var completedAtPrematurely = DateTimeOffset.Now;
                var dummInteraction = new UserBotInteraction
                {
                    UserId = userid,
                    BotId = botid,
                    UserQuery = query,
                    InitiatedAt = iniatedAt,
                    BotResponse = "",
                    BotAwareness = "s",
                    BotStatefulness = "0",
                    CompletedAt = completedAtPrematurely,
                    IsComplete = true

                };
                return dummInteraction;
            }

            string result = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine("Result: " + result);

            Console.ReadLine();
            var completedAt = DateTimeOffset.Now;

            var interaction1 = new UserBotInteraction
            {
                UserId = userid,
                BotId = botid,
                UserQuery = query,
                InitiatedAt = iniatedAt,
                BotResponse = result,
                BotAwareness = "s",
                BotStatefulness = "0",
                CompletedAt = completedAt,
                IsComplete = true

            };
            _algoromidaContext.Add(interaction1);
            var saveResult = await _algoromidaContext.SaveChangesAsync();
            Console.WriteLine(saveResult);
            //return saveResult == 1;
            return interaction1;
        }
    }
}
