using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MakauTech.Data;
using MakauTech.Models;

namespace MakauTech.Controllers
{
    public class AdminController : Controller
    {
        private readonly MakauTechDbContext _context;
        private readonly ILogger<AdminController> _logger;

        public AdminController(MakauTechDbContext context, ILogger<AdminController> logger)
        {
            _context = context;
            _logger = logger;
        }

        private bool IsAdmin()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return false;
            var user = _context.Users.Find(userId);
            return user is Admin;
        }

        private void SetViewBag()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var user = userId != null ? _context.Users.Find(userId) : null;
            ViewBag.IsLoggedIn = user != null;
            ViewBag.UserName = user?.Name ?? "";
            ViewBag.UserPoints = user?.Points ?? 0;
            ViewBag.UserLevel = user?.Level ?? "";
        }

        // GET: /Admin/Dashboard
        public IActionResult Dashboard()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Home");
            SetViewBag();
            ViewBag.TotalPlaces = _context.Places.Count();
            ViewBag.TotalUsers = _context.Users.Count(u => u.Email != "admin@makautech.com");
            ViewBag.TotalReviews = _context.Reviews.Count();
            ViewBag.TotalCategories = _context.Categories.Count();
            ViewBag.TotalAchievements = _context.Achievements.Count();
            try { ViewBag.TotalLikes = _context.PlaceLikes.Count(); } catch { ViewBag.TotalLikes = 0; }
            ViewBag.TopPlaces = _context.Places.OrderByDescending(p => p.VisitCount).Take(5).ToList();
            ViewBag.RecentReviews = _context.Reviews.Include(r => r.Place).OrderByDescending(r => r.CreatedAt).Take(5).ToList();
            ViewBag.TopUsers = _context.Users.Where(u => u.Email != "admin@makautech.com").OrderByDescending(u => u.Points).Take(5).ToList();
            return View();
        }

        // ── PLACES ──────────────────────────────────────────
        public IActionResult Places()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Home");
            SetViewBag();
            var places = _context.Places.Include(p => p.Category).ToList();
            return View(places);
        }

        public IActionResult CreatePlace()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Home");
            SetViewBag();
            ViewBag.Categories = _context.Categories.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreatePlace(string name, string location, string description, string imageUrl, double rating, int categoryId)
        {
            try
            {
                if (!IsAdmin()) return RedirectToAction("Login", "Home");
                _context.Places.Add(new Place { Name = name, Location = location, Description = description, ImageUrl = imageUrl, Rating = rating, CategoryId = categoryId });
                _context.SaveChanges();
                return RedirectToAction("Places");
            }
            catch (Exception) { return RedirectToAction("Places"); }
        }

        public IActionResult EditPlace(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Home");
            SetViewBag();
            var place = _context.Places.Find(id);
            if (place == null) return NotFound();
            ViewBag.Categories = _context.Categories.ToList();
            return View(place);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditPlace(int id, string name, string location, string description, string imageUrl, double rating, int categoryId)
        {
            try
            {
                if (!IsAdmin()) return RedirectToAction("Login", "Home");
                var place = _context.Places.Find(id);
                if (place == null) return NotFound();
                place.Name = name; place.Location = location; place.Description = description;
                place.ImageUrl = imageUrl; place.Rating = rating; place.CategoryId = categoryId;
                _context.SaveChanges();
                return RedirectToAction("Places");
            }
            catch (Exception) { return RedirectToAction("Places"); }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePlace(int id)
        {
            try
            {
                if (!IsAdmin()) return RedirectToAction("Login", "Home");
                var place = _context.Places.Find(id);
                if (place != null) { _context.Places.Remove(place); _context.SaveChanges(); }
                return RedirectToAction("Places");
            }
            catch (Exception) { return RedirectToAction("Places"); }
        }

        // ── CATEGORIES ──────────────────────────────────────
        public IActionResult Categories()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Home");
            SetViewBag();
            return View(_context.Categories.ToList());
        }

        public IActionResult CreateCategory()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Home");
            SetViewBag();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateCategory(string name, string icon)
        {
            try
            {
                if (!IsAdmin()) return RedirectToAction("Login", "Home");
                _context.Categories.Add(new Category { Name = name, Icon = icon });
                _context.SaveChanges();
                return RedirectToAction("Categories");
            }
            catch (Exception) { return RedirectToAction("Categories"); }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteCategory(int id)
        {
            try
            {
                if (!IsAdmin()) return RedirectToAction("Login", "Home");
                var cat = _context.Categories.Find(id);
                if (cat != null) { _context.Categories.Remove(cat); _context.SaveChanges(); }
                return RedirectToAction("Categories");
            }
            catch (Exception) { return RedirectToAction("Categories"); }
        }

        // ── REVIEWS ─────────────────────────────────────────
        public IActionResult Reviews()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Home");
            SetViewBag();
            var reviews = _context.Reviews.Include(r => r.Place).ToList();
            return View(reviews);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteReview(int id)
        {
            try
            {
                if (!IsAdmin()) return RedirectToAction("Login", "Home");
                var review = _context.Reviews.Find(id);
                if (review != null) { _context.Reviews.Remove(review); _context.SaveChanges(); }
                return RedirectToAction("Reviews");
            }
            catch (Exception) { return RedirectToAction("Reviews"); }
        }

        // ── USERS ────────────────────────────────────────────
        public IActionResult Users()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Home");
            SetViewBag();
            return View(_context.Users.ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteUser(int id)
        {
            try
            {
                if (!IsAdmin()) return RedirectToAction("Login", "Home");
                var user = _context.Users.Find(id);
                if (user != null) { _context.Users.Remove(user); _context.SaveChanges(); }
                return RedirectToAction("Users");
            }
            catch (Exception) { return RedirectToAction("Users"); }
        }

        // ── ACHIEVEMENTS ─────────────────────────────────────
        public IActionResult Achievements()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Home");
            SetViewBag();
            return View(_context.Achievements.ToList());
        }

        public IActionResult CreateAchievement()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Home");
            SetViewBag();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateAchievement(string name, string description, string icon, int pointsRequired, int placesRequired)
        {
            try
            {
                if (!IsAdmin()) return RedirectToAction("Login", "Home");
                _context.Achievements.Add(new Achievement { Name = name, Description = description, Icon = icon, PointsRequired = pointsRequired, PlacesRequired = placesRequired });
                _context.SaveChanges();
                return RedirectToAction("Achievements");
            }
            catch (Exception) { return RedirectToAction("Achievements"); }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteAchievement(int id)
        {
            try
            {
                if (!IsAdmin()) return RedirectToAction("Login", "Home");
                var a = _context.Achievements.Find(id);
                if (a != null) { _context.Achievements.Remove(a); _context.SaveChanges(); }
                return RedirectToAction("Achievements");
            }
            catch (Exception) { return RedirectToAction("Achievements"); }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ResetLeaderboard()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Home");
            MakauTech.Data.DbSeeder.AdminResetLeaderboard(_context);
            TempData["AdminSuccess"] = "Leaderboard reset — all user points, badges and visits cleared.";
            return RedirectToAction("Users");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult FactoryReset()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Home");
            try
            {
                // 1. Delete all reviews
                _context.Database.ExecuteSqlRaw(@"DELETE FROM ""Reviews""");

                // 2. Delete all place likes
                _context.Database.ExecuteSqlRaw(@"DELETE FROM ""PlaceLikes""");

                // 3. Delete all non-admin users (Discriminator != 'Admin')
                _context.Database.ExecuteSqlRaw(
                    @"DELETE FROM ""Users"" WHERE ""Discriminator"" != 'Admin'");

                // 4. Reset place visit counts and ratings
                _context.Database.ExecuteSqlRaw(
                    @"UPDATE ""Places"" SET ""VisitCount"" = 0, ""Rating"" = 0");

                // 5. Reset auto-increment IDs for wiped tables
                _context.Database.ExecuteSqlRaw(
                    @"DELETE FROM ""sqlite_sequence"" WHERE ""name"" IN ('Reviews','PlaceLikes','Users')");

                _context.SaveChanges();
                TempData["AdminSuccess"] = "Factory reset complete — all user data wiped. Admin account preserved.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Factory reset failed");
                TempData["AdminError"] = "Factory reset failed. Check server logs for details.";
            }
            return RedirectToAction("Dashboard");
        }
    }
}