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


        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Challenge();
            }

            var prevInteractions = await _algoromidaService.GetPreviousAsync(currentUser);

            var model = new UserBotInteractionViewModel()
            {
                Interactions = prevInteractions
            };

            return View(model);
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Respond(UserBotInteraction _userBotInteraction)
        {
            
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index", _userBotInteraction.BotId);
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {                
                return RedirectToAction("Index", _userBotInteraction.BotId);
            }

            bool successful = await _algoromidaService.RespondAsync(_userBotInteraction, currentUser);
            if (!successful)
            {               
                return BadRequest("Could not communicate with " + _userBotInteraction.BotId);
            }

            return RedirectToAction("Index");
        }
    }
}
