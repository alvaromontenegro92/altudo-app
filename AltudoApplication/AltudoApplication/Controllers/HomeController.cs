using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AltudoApplication.Models;
using AltudoApplication.Business;
using AltudoApplication.WebApp.Models;
using System.Net.Http;

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
            return View(new FormRequest());
        }
        public IActionResult ElementExtractor(FormRequest formRequest)
        {
            var response = new HttpResponse();

            if (formRequest.URL == null)
            {
                response = new HttpResponse()
                {
                    Response = new HttpResponseMessage(System.Net.HttpStatusCode.NotFound)
                    ,
                    Content = String.Empty
                };
            }
            else
            {
                var url = new Uri(formRequest.URL);
                var htmlManager = new HtmlManager();
                response = htmlManager.GetSourceCodeFromWebSite(url);

                if (response.Response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var images = new ImageExtractor();
                    var words = new TextManipulation();

                    ViewBag.SubTitle = url.AbsoluteUri;
                    ViewBag.Images = images.ExtractImagesFromSourceCode(url, response.Content);
                    ViewBag.Words = words.GetTOP10Words(url, response.Content);
                }
            }                

            if (response.Response.StatusCode == System.Net.HttpStatusCode.OK)             
                return View();
            else
            {
                TempData["ErroAutenticacao"] = "URL Invalida. Tente novamente!";
                return RedirectToAction("Index", "Home");
            }
            
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
