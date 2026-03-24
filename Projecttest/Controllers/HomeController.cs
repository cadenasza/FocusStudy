using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Projecttest.Models;
using Projecttest.Data;
using System.Linq;

namespace Projecttest.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SaveSession(int durationMinutes, string? subject, int? targetMinutes, string? badge)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return Unauthorized();
            }

            if (durationMinutes <= 0)
            {
                return BadRequest("Duração inválida.");
            }

            var now = DateTime.Now;
            var startTime = now.AddMinutes(-durationMinutes);
            var endTime = now;

            var session = new StudySession
            {
                UserId = userId.Value,
                StartTime = startTime,
                EndTime = endTime,
                DurationMinutes = durationMinutes,
                Subject = subject,
                TargetMinutes = targetMinutes,
                Badge = string.IsNullOrEmpty(badge) ? "Foco" : badge
            };

            _context.StudySessions.Add(session);
            _context.SaveChanges();

            return RedirectToAction("History");
        }

        public IActionResult History(string searchString)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var sessionsQuery = _context.StudySessions
                .Where(s => s.UserId == userId.Value);

            if (!string.IsNullOrEmpty(searchString))
            {
                sessionsQuery = sessionsQuery.Where(s => s.Subject != null && s.Subject.Contains(searchString));
            }

            var sessions = sessionsQuery
                .OrderByDescending(s => s.StartTime)
                .ToList();

            ViewData["CurrentFilter"] = searchString;

            return View(sessions);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
