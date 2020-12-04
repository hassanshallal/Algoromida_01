using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Algoromida_01.Models;
using Algoromida_01.Services;

namespace Algoromida_01.Controllers
{
    public class AlgoromidaController : Controller
    {
        private readonly IAlgoromidaService _algoromidaService;

        public AlgoromidaController(IAlgoromidaService algoromidaService)
        {
            _algoromidaService = algoromidaService;
        }

        public async Task<IActionResult> Index()
        {
            var thisInteraction = await _algoromidaService.RespondAsync("jjsx", "Dona", "Hola");
            var model = new UserBotInteractionViewModel()
            {
                ThisInteraction = thisInteraction
            };
            return View(model);
        }
    }      
}
