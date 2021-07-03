using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AltudoApplication.Models;
using AltudoApplication.Bussiness;
using AltudoApplication.WebApp.Models;

namespace AltudoApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var formRequest = new FormRequest();
            return View(formRequest);
        }
        public IActionResult ElementExtractor(FormRequest formRequest)
        {
            var url = new Uri(formRequest.URL);
            //var url = new Uri("https://ge.globo.com/olimpiadas/noticia/brasil-vence-o-mexico-e-vai-a-final-do-pre-olimpico-de-basquete.ghtml");
            //var url = new Uri("http://www.graintek.com.br/servicos");

            var images = new ImageExtractor();
            ViewBag.Images = images.ExtractImagesFromWebSite(url);   
            
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
