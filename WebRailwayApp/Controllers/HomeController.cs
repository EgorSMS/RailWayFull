using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebRailwayApp.Models;

namespace WebRailwayApp.Controllers
{

    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private RailwayDBContext db;

        public HomeController(ILogger<HomeController> logger, RailwayDBContext context)
        {
            _logger = logger;
            this.db = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddCity()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddCity(Cities city)
        {
            if (ModelState.IsValid)
            {
                db.Cities.Add(city);
                await db.SaveChangesAsync();
                return Redirect("Index"); //ПОМЕНЯТЬ ВЫВОД НА СПИСОК ГОРОДОВ
            }
            else return View(city);
        }

        public async Task<IActionResult> AddRoute()
        {
            RouteDisplay routeDisplay = new RouteDisplay();
            routeDisplay.cities = await db.Cities.ToListAsync();
            return View(routeDisplay);
        }

        [HttpPost]
        public async Task<IActionResult> AddRoute(RouteDisplay routeDisplay)
        {
            routeDisplay.cities = await db.Cities.ToListAsync();
            if (ModelState.IsValid)
            {
                routeDisplay.ErrorDeparturePlatform = false;
                routeDisplay.ErrorArrivePlatform = false;
                if (routeDisplay.route.PlatformDeparture > db.Cities.FirstOrDefaultAsync(c => c.ID_City == routeDisplay.route.ID_City_Departure).Result.PlatformCount)
                    routeDisplay.ErrorDeparturePlatform = true;
                if (routeDisplay.route.PlatformArrival > db.Cities.FirstOrDefaultAsync(c => c.ID_City == routeDisplay.route.ID_City_Arrival).Result.PlatformCount)
                    routeDisplay.ErrorArrivePlatform = true;

                if (routeDisplay.ErrorArrivePlatform || routeDisplay.ErrorDeparturePlatform) return View(routeDisplay);

                db.Route.Add(routeDisplay.route);
                await db.SaveChangesAsync();
                return Redirect("Index"); //ПОМЕНЯТЬ ВЫВОД НА СПИСОК МАРШРУТОВ
            }
            else return View(routeDisplay);
        }

        public IActionResult TimeTable()
        {
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Authorization", "Auth");
        }

        // ОБНОВЛЕНИЕ

        public async Task<IActionResult> AddStaff()
        {
            staffDisplay staffDisplay = new staffDisplay();
            staffDisplay.cities = await db.Cities.ToListAsync();
            return View(staffDisplay);
        }

        [HttpPost]
        public async Task<IActionResult> AddStaff(staffDisplay staffDisplay)
        {
            staffDisplay.cities = await db.Cities.ToListAsync();

            return View(staffDisplay);
        }

        public async Task<IActionResult> DeleteStaff(int id)
        {
            var staffToDelete = await db.staff.FirstAsync(s => s.ID_Staff == id);
            if (staffToDelete != null)
            {
                db.staff.Remove(staffToDelete);
                await db.SaveChangesAsync();
            }
            return RedirectToAction("InfoStaff");
        }

        public async Task<IActionResult> InfoStaff()
        {
            return View(await db.staff.ToListAsync());
        }

        public IActionResult AddTimeTable()
        {
            return View();
        }
    }
}
