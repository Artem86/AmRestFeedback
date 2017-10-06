using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AmRestFeedback.Models.HomeViewModels;
using System.Globalization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Authorization;
using AmRestFeedback.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using AmRestFeedback.Models;

namespace AmRestFeedback.Controllers
{
    public class HomeController : Controller
    {
        private readonly IStringLocalizer<HomeController> _localizer;
        private readonly ApplicationDbContext _dbcontext;

        public HomeController(IStringLocalizer<HomeController> localizer, ApplicationDbContext dbcontext)
        {
            _localizer = localizer;
            _dbcontext = dbcontext;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitFeedback(HomeViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Index", model);

            var db = _dbcontext;
            db.Feedbacks.Add(new Feedback
            {
                Name = model.Name,
                Description = model.Description
            });
            await db.SaveChangesAsync();
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
