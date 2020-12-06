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

        [HttpGet("{query}", Name = "RespondAsync")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string query)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }
            var CurrentInteraction = await _algoromidaService.RespondAsync("jjsx", "Dona", query);
            return View(CurrentInteraction);
        }
    }      
}
