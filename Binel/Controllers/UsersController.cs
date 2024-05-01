using Microsoft.AspNetCore.Mvc;
using Binel.Models;
using System.Threading.Tasks;

namespace Binel.Controllers
{
    public class UsersController : Controller
    {
        private readonly BinelProjectContext _context;

        public UsersController(BinelProjectContext context)
        {
            _context = context;
        }

        // GET: /register
        public IActionResult Register()
        {
            return View();
        }

        // POST: /register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Name, Surname, Email, Phone, Birth, PasswordHash")] User user)
        {
            if (ModelState.IsValid)
            {
                // Ekstra doğrulamalar yapabilirsiniz (şifre kontrolü, e-posta doğrulama vb.)

                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(RegisterConfirmation));
            }
            return View(user);
        }

        // GET: /register/confirmation
        public IActionResult RegisterConfirmation()
        {
            return View();
        }
    }
}
