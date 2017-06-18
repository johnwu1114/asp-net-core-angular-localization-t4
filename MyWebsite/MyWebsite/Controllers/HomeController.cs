﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MyWebsite.Middlewares;
using System.Globalization;

namespace MyWebsite.Controllers
{
    [MiddlewareFilter(typeof(CultureMiddleware))]
    public class HomeController : Controller
    {
        private readonly IStringLocalizer _localizer;

        public HomeController(IStringLocalizer<HomeController> localizer)
        {
            _localizer = localizer;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Content()
        {
            return Content($"CurrentCulture: {CultureInfo.CurrentCulture.Name}\r\n"
                         + $"CurrentUICulture: {CultureInfo.CurrentUICulture.Name}\r\n"
                         + $"Hello: {_localizer["Hello"]}");
        }
    }
}