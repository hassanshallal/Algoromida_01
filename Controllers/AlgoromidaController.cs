using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

using Algoromida_01.Models;
using Algoromida_01.Services;


namespace Algoromida_01.Controllers
{
    [Authorize]
    public class AlgoromidaController : Controller
    {
        private readonly IAlgoromidaService _algoromidaService;
        private readonly UserManager<AlgoromidaUser> _userManager;

        public AlgoromidaController(IAlgoromidaService algoromidaService, UserManager<AlgoromidaUser> userManager)
        {
            _algoromidaService = algoromidaService;
            _userManager = userManager;
        }


        public async Task<IActionResult> Index(string botId)
        {

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Challenge();
            }

            var prevInteractions = await _algoromidaService.GetPreviousAsync(currentUser, botId);

            var model = new UserBotInteractionViewModel()
            {
                BotId = botId,
                Interactions = prevInteractions
            };

            return View(model);
        }


        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Respond(UserBotInteraction _userBotInteraction, string botId)
        {
	        _userBotInteraction.BotId = botId;
            
            if (!ModelState.IsValid)
            {
		        return RedirectToAction("Index", "Algoromida", new { botId = _userBotInteraction.BotId });
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {                
                return RedirectToAction("Index", "Home");
            }

            bool successful = await _algoromidaService.RespondAsync(_userBotInteraction, currentUser);
            if (!successful)
            {               
                return BadRequest("Could not communicate with " + _userBotInteraction.BotId + " at the moment...");
            }
	    
	        return RedirectToAction("Index", "Algoromida", new { botId = _userBotInteraction.BotId });
        }
    }
}
