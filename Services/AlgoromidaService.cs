using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Hosting;

using System.Net;
using System.Net.Http;
using System.Text.Json;

using Algoromida_01.Models;
using Algoromida_01.Authenticate;

using System.Text;


namespace Algoromida_01.Services
{
    public class AlgoromidaService : IAlgoromidaService
    {
        private readonly AlgoromidaContext _algoromidaContext;
        public AuthAlgoromidaOptions _options { get; }

        public AlgoromidaService(AlgoromidaContext algoromidaContext, IOptions<AuthAlgoromidaOptions> optionsAccessor)
        {
            _algoromidaContext = algoromidaContext;
            _options = optionsAccessor.Value;
        }

        private async Task<string> PreparePrevInteractions(AlgoromidaUser user, string botId)
        {
            // Get PrevIntearctions
            UserBotInteraction[] PrevIntearctions = await GetPreviousAsync(user, botId);

            // Iterate
            string previous = "";
            if (PrevIntearctions.Length > 0)
            {
                foreach (var interaction in PrevIntearctions)
                {
                    previous = previous + interaction.UserId + "<>" + interaction.UserQuery + "<>" + interaction.BotResponse + "_";
                }
                previous = previous.Remove(previous.Length - 1, 1);
            }

            return previous;
        }

        private async Task<Dictionary<string, string>> PreparePayload(UserBotInteraction userBotInteraction, AlgoromidaUser user)
        {
            string previous = await PreparePrevInteractions(user, userBotInteraction.BotId);
            var primer = new Dictionary<string, string>
            {
                ["botName"] = userBotInteraction.BotId,
                ["text"] = userBotInteraction.UserQuery,
                ["senderInfo"] = ("(" + user.Id + ", ") + (user.FirstName + ", ") + (user.LastName + ", ") + (user.TimeZone + ", ") + (user.Gender + ", ") + ("Algoromida)"),
                ["prevInteactions"] = previous
            };

            
            return primer;
        }

        private Dictionary<string, string> SendReceive(Dictionary<string, string> primer)
        {
            var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate });
            if (primer["botName"] == "Dona")
            {
                Console.WriteLine("DonaTunnel");
                client.BaseAddress = new Uri(_options.DonaTunnel);
            }
            else if (primer["botName"] == "Thermogena")
            {
                Console.WriteLine("ThermogenaTunnel");
                client.BaseAddress = new Uri(_options.ThermogenaTunnel);
            }

            string payload = JsonSerializer.Serialize(primer);
            HttpContent body = new StringContent(payload, Encoding.UTF8, "application/json");
            Console.Write(body);
            HttpResponseMessage response = client.PostAsync(client.BaseAddress, body).Result;

            var successful = response.IsSuccessStatusCode;
            if (!successful)
            {
                var unsuccessful = new Dictionary<string, string>
                {
                    ["BotResponse"] = "",
                    ["BotAwareness"] = "",
                    ["BotStatefulness"] = "",
                    ["isSuccessful"] = "false"
                };
                return unsuccessful;
            }

            string result = response.Content.ReadAsStringAsync().Result;
            var results = JsonSerializer.Deserialize<Dictionary<string, string>>(result);
            results["isSuccessful"] = "true";
            return results;
        }

        public async Task<UserBotInteraction[]> GetPreviousAsync(AlgoromidaUser user, string botId)
        {
            Console.Write("From Algoromida Service GetPreviousAsync: " + botId);
            Console.WriteLine();

            return await _algoromidaContext.Interactions.Where(x => x.UserId == user.Id && x.BotId == botId).ToArrayAsync();
        }

        public async Task<bool> RespondAsync(UserBotInteraction userBotInteraction, AlgoromidaUser user)
        {
            Console.WriteLine("Respond entered");
            userBotInteraction.UserId = user.Id;
	    userBotInteraction.InitiatedAt = DateTimeOffset.Now.AddHours(Int32.Parse(user.TimeZone));
            userBotInteraction.BotAvatarPath = "/images/" + userBotInteraction.BotId + ".jpeg";
            userBotInteraction.UserAvatarPath = "/uploads/" + user.AvatarPath;

            Dictionary<string, string> primer = await PreparePayload(userBotInteraction, user);
            Console.WriteLine("botname from primer" + primer["botName"]);

            var results = SendReceive(primer);
            
            userBotInteraction.BotResponse = results["BotResponse"];
            userBotInteraction.BotAwareness = results["BotAwareness"];
            userBotInteraction.BotStatefulness = results["BotStatefulness"];
	    userBotInteraction.CompletedAt = DateTimeOffset.Now.AddHours(Int32.Parse(user.TimeZone));
            if (results["isSuccessful"] == "true")
            {
                userBotInteraction.IsSuccessful = true;
            }
            else
            {
                userBotInteraction.IsSuccessful = false;
            }

            _algoromidaContext.Add(userBotInteraction);
            var saveResult = await _algoromidaContext.SaveChangesAsync();
            return saveResult == 1;
        }
    }
}
