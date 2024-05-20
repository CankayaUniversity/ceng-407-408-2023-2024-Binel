using Microsoft.AspNetCore.Mvc;
using Binel.Models;
using Binel.ViewModels;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

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
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Password ve ConfirmPassword alanlarını kontrol et
                if (model.Password != model.ConfirmPassword)
                {
                    ModelState.AddModelError("ConfirmPassword", "The password and confirmation password do not match.");
                    return View(model);
                }

                // Parolayı hashle
                string hashedPassword = ComputeSHA256Hash(model.Password);

                // Tarih tipini doğrudan DateOnly'e dönüştür
                DateOnly birthDate = new DateOnly(model.Birth.Year, model.Birth.Month, model.Birth.Day);

                // Yeni kullanıcı oluştur ve kaydet
                var user = new User
                {
                    Name = model.Name,
                    Surname = model.Surname,
                    Email = model.Email,
                    Phone = model.Phone,
                    Birth = birthDate,
                    PasswordHash = hashedPassword
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(RegisterConfirmation));
            }

            return View(model);
        }
        #region
        // Duygu start
        public async Task<IActionResult> Edit()
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var userId))
            {

                return RedirectToAction("Login");
            }

            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                return View(user);
            }
            else
            {
                return RedirectToAction("Login");
            }

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int userId, User user)
        {
            ViewBag.OrganizationId = user.OrganizationId;
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Login");

                }
                catch (Exception ex)
                {

                    ModelState.AddModelError(string.Empty, "Bir hata oluştu, lütfen daha sonra tekrar deneyin.");
                }

            }

            return View(user);
        }
        // Duygu finish
        // Metod parolayı SHA256 ile hashlemek için
        private string ComputeSHA256Hash(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha256.ComputeHash(bytes);

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2")); // Hexadecimal format
                }

                return builder.ToString();
            }
        }

        // GET: /register/confirmation
        public IActionResult RegisterConfirmation()
        {
            return View();
        }
// GET: /login
public IActionResult Login()
{
    return View();
}

// POST: /login
[HttpPost]
//[ValidateAntiForgeryToken]
public async Task<IActionResult> Login(LoginViewModel model)
{
    if (ModelState.IsValid)
    {
        // Burada giriş işlemlerini gerçekleştirin, örneğin, kullanıcıyı veritabanında arayın ve kimlik doğrulaması yapın

        // Örnek bir kimlik doğrulama
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
         string hashedPassword1=""; 
        if (user != null)
        {
            // Kullanıcı bulundu, şimdi şifreyi karşılaştırın
            string hashedPassword = ComputeSHA256Hash(model.Password);
            hashedPassword1 = hashedPassword;
            if (user.PasswordHash == hashedPassword)
            {
                        var claims = new List<Claim>
{
    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString())
};
                        var identity = new ClaimsIdentity(claims, "login");
                        var principal = new ClaimsPrincipal(identity);
                        await HttpContext.SignInAsync(principal);
                        // Şifre eşleşti, giriş başarılı
                        // Bu aşamada genellikle oturum açma işlemi gerçekleştirilir veya kullanıcı bilgileri saklanır
                        // Örnek olarak TempData kullanarak bir bilgi saklayabilirsiniz
                        TempData["Message"] = "Login successful.";
                return RedirectToAction(nameof(LoginConfirmation));
            }
        }
        TempData["Message"] = "Login error.:";
        // Kullanıcı adı veya şifre hatalı
        ModelState.AddModelError(string.Empty, "Invalid username or password.");
    }

    // Eğer ModelState.IsValid false ise veya kimlik doğrulama başarısız olduysa, tekrar login sayfasını göster
    return View(model);
}
        
        #endregion
        // GET: /login/confirmation
        public IActionResult LoginConfirmation()
{
    return View();
}


    }
}
