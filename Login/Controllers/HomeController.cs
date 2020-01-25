﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Login.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

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
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
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
        [Authorize]

        public IActionResult Secret()
        {
            return View();
        }
        [Authorize(Policy = "Claim.DOB")]
        public IActionResult SecretPolicy()
        {
            return View("Secret");
        }
        [Authorize(Roles  = "Admin")]
        public IActionResult SecretRole()
        {
            return View("Secret");
        }
        public IActionResult Authenticate()
        {
            var grandmaClaim = new List<Claim>
            {
                new Claim(ClaimTypes.Name ,"Bob"),
                new Claim(ClaimTypes.Email ,"Bob@hotmail.com"),
                new Claim(ClaimTypes.DateOfBirth ,"11/11/2020"),
                new Claim(ClaimTypes.Role ,"Admin"),
                new Claim("GrandmaSays" ,"Very nice boi")
            };
            var licenseClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name ,"Bob k Fo"),
                new Claim("DrivingLicense" ,"A+")
            };

            var grandmaIdentity = new ClaimsIdentity(grandmaClaim, "Grandma Identity");
            var licenseIdentity = new ClaimsIdentity(licenseClaims, "Government");
            var userPrincipal = new ClaimsPrincipal(new[] { grandmaIdentity, licenseIdentity });
            HttpContext.SignInAsync(userPrincipal);
            return RedirectToAction("Index");
        }
        [Authorize]

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
