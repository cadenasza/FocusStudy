using Microsoft.AspNetCore.Mvc;
using Projecttest.Data;
using Projecttest.Models;
using System.Security.Cryptography;
using System.Text;

namespace Projecttest.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Register() => View();

        [HttpPost]
        public IActionResult Register(string name, string email, string password)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "Preencha todos os campos.";
                return View();
            }

            if (_context.Users.Any(u => u.Email == email))
            {
                ViewBag.Error = "E-mail já cadastrado.";
                return View();
            }

            var user = new User
            {
                Name = name,
                Email = email,
                PasswordHash = HashPassword(password)
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserName", user.Name);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var hash = HashPassword(password);
            var user = _context.Users.FirstOrDefault(u => u.Email == email && u.PasswordHash == hash);

            if (user == null)
            {
                ViewBag.Error = "Usuário ou senha inválidos.";
                return View();
            }

            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserName", user.Name);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        private static string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }
}
