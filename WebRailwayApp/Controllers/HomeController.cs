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
            return View(city);
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
            return View(routeDisplay);
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

        public async Task<IActionResult> AddStaff()
        {
            staffDisplay staffDisplay = new staffDisplay();
            staffDisplay.doljnosts = await db.Doljnost.ToListAsync();
            return View(staffDisplay);
        }

        [HttpPost]
        public async Task<IActionResult> AddStaff(staffDisplay staffDisplay)
        {
            staffDisplay.doljnosts = await db.Doljnost.ToListAsync();
            if (ModelState.IsValid)
            {
                db.staff.Add(staffDisplay.staff);
                await db.SaveChangesAsync();
                return RedirectToAction("InfoStaff");
            }
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

        public async Task<IActionResult> AddTimeTable()
        {
            TimeTableDisplay timeTableDisplay = new TimeTableDisplay();
            timeTableDisplay.cities = await db.Cities.ToListAsync();
            timeTableDisplay.trains = await db.Train.ToListAsync();
            timeTableDisplay.routes = await db.Route.ToListAsync();
            return View(timeTableDisplay);
        }

        [HttpPost]
        public async Task<IActionResult> AddTimeTable(TimeTableDisplay timeTableDisplay)
        {
            timeTableDisplay.cities = await db.Cities.ToListAsync();
            timeTableDisplay.trains = await db.Train.ToListAsync();
            var routes = await db.Route.ToListAsync();
            timeTableDisplay.routes = routes;
            if (ModelState.IsValid)
            {
                if (timeTableDisplay.timeTable.DateTimeArrived <= timeTableDisplay.timeTable.DateTimeDeparted) 
                    timeTableDisplay.ErrorDate = true;

                DateTime dateS = timeTableDisplay.timeTable.DateTimeDeparted;
                DateTime dateE = timeTableDisplay.timeTable.DateTimeArrived;

                var stops = await db.Stops.ToListAsync();
                var route = routes.FirstOrDefault(r => r.ID_Route == timeTableDisplay.timeTable.ID_Route);
                foreach (Stops stop in stops)
                {
                    if (stop.ID_City == route.ID_City_Departure && dateS < stop.TimeOfStop.AddMinutes(30) && dateS > stop.TimeOfStop.AddMinutes(-30) && stop.Platform == route.PlatformDeparture)
                        timeTableDisplay.ErrorDateDepartureAlreadyExist = true;

                    if (stop.ID_City == route.ID_City_Arrival && dateE < stop.TimeOfStop.AddMinutes(30) && dateE > stop.TimeOfStop.AddMinutes(-30) && stop.Platform == route.PlatformArrival)
                        timeTableDisplay.ErrorDateArrivalAlreadyExist = true;
                }

                var timeTables = await db.TimeTable.ToListAsync();
                foreach (TimeTable timeT in timeTables)
                {
                    var routeCheck = routes.Where(r => r.ID_Route == timeT.ID_Route).FirstOrDefault();

                    if (routeCheck.ID_City_Departure == route.ID_City_Departure && dateS < timeT.DateTimeDeparted.AddMinutes(30) && dateS > timeT.DateTimeDeparted.AddMinutes(-30) && routeCheck.PlatformDeparture == route.PlatformDeparture)
                        timeTableDisplay.ErrorDateDepartureAlreadyExist = true;

                    if (routeCheck.ID_City_Arrival == route.ID_City_Departure && dateS < timeT.DateTimeArrived.AddMinutes(30) && dateS > timeT.DateTimeArrived.AddMinutes(-30) && routeCheck.PlatformArrival == route.PlatformDeparture)
                        timeTableDisplay.ErrorDateDepartureAlreadyExist = true;

                    if (routeCheck.ID_City_Departure == route.ID_City_Arrival && dateE < timeT.DateTimeDeparted.AddMinutes(30) && dateE > timeT.DateTimeDeparted.AddMinutes(-30) && routeCheck.PlatformDeparture == route.PlatformArrival)
                        timeTableDisplay.ErrorDateArrivalAlreadyExist = true;

                    if (routeCheck.ID_City_Arrival == route.ID_City_Arrival && dateE < timeT.DateTimeArrived.AddMinutes(30) && dateE > timeT.DateTimeArrived.AddMinutes(-30) && routeCheck.PlatformArrival == route.PlatformArrival)
                        timeTableDisplay.ErrorDateArrivalAlreadyExist = true;
                }

                if (timeTableDisplay.ErrorDateArrivalAlreadyExist || timeTableDisplay.ErrorDateDepartureAlreadyExist || timeTableDisplay.ErrorDate)
                    return View(timeTableDisplay);

                db.TimeTable.Add(timeTableDisplay.timeTable);
                await db.SaveChangesAsync();
                return Redirect("Index"); //ПОМЕНЯТЬ ВЫВОД НА СПИСОК РАСПИСАНИЯ
            }
            return View(timeTableDisplay);
        }

        [HttpPost]
        public async Task<IActionResult> AddStopTimeTable(TimeTableDisplay timeTableDisplay)
        {
            timeTableDisplay.cities = await db.Cities.ToListAsync();
            timeTableDisplay.trains = await db.Train.ToListAsync();
            timeTableDisplay.routes = await db.Route.ToListAsync();


            return View("AddTimeTable", timeTableDisplay);
        }
    }
}
