using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Login.Models;

namespace Login.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly GooglereCaptchaService googlereCaptchaService;

        public HomeController(ILogger<HomeController> logger,GooglereCaptchaService googlereCaptchaService)
        {
            _logger = logger;
            this.googlereCaptchaService = googlereCaptchaService;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(UserModel model)
        {
            //google rcp
            var grcp = googlereCaptchaService.VerifyreCaptcha(model.Token);
            if(!grcp.Result.success && grcp.Result.score <= 0.5)
            {
                ModelState.AddModelError(string.Empty,"Your are Not human....");
                return View(model);
            }
            if (ModelState.IsValid)
            {

            }
            return View(model);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
