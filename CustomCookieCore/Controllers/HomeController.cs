﻿using CustomCookieCore.Data;
using CustomCookieCore.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CustomCookieCore.Controllers
{
    public class HomeController : Controller
    {
        private readonly CookieContext _context;

        public HomeController(CookieContext context)
        {
            _context = context;
        }

        public IActionResult SignIn()
        {
            return View(new UserSignInModel());
        }
        [HttpPost]
        public async Task<IActionResult> SignIn(UserSignInModel signInModel)
        {
            //bu alan başarılı olursa gerçekleşir
            var user = _context.AppUsers.SingleOrDefault(x => x.UserName == signInModel.UserName && x.Password == signInModel.Password);
            if (user != null)
            {
                var roles = _context.AppRoles.Where(x => x.UserRoles.Any(x => x.UserId == user.Id)).Select(x=>x.Definition).ToList();
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, signInModel.UserName),

        };
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = signInModel.Remember
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);
                return RedirectToAction("Index");

            }
            ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı");
            return View(signInModel);

        }

        public IActionResult Index()
        {
            return View();
        }
        [Authorize(Roles ="Admin")]
        public IActionResult Admin()
        {
            return View();
        }
        [Authorize(Roles = "Admin,Member")]
        public IActionResult Member()
        {
            return View();
        }
        public IActionResult AccesDenied()
        {
            return View();
        }
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(
        CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("SignIn");
        }




    }
}
