using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Algoromida_01.Models;

namespace Algoromida_01.Services
{
    public interface IAlgoromidaService
    {
        Task<UserBotInteraction[]> GetPreviousAsync(string userid, string botid);

        Task<UserBotInteraction> RespondAsync(string query, string userid, string botid);
    }
}
