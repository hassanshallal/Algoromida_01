using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Algoromida_01.Models;

namespace Algoromida_01.Services
{
    public interface IAlgoromidaService
    {
        Task<UserBotInteraction[]> GetPreviousAsync(AlgoromidaUser user, string botid);

        Task<bool> RespondAsync(UserBotInteraction userBotInteraction, AlgoromidaUser user);

    }
}
