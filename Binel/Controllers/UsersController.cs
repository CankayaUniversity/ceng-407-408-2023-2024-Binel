using Microsoft.AspNetCore.Mvc;
using Binel.Models;
using Binel.ViewModels;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

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
                // Şifre eşleşti, giriş başarılı
                // Bu aşamada genellikle oturum açma işlemi gerçekleştirilir veya kullanıcı bilgileri saklanır
                // Örnek olarak TempData kullanarak bir bilgi saklayabilirsiniz
                TempData["Message"] = "Login successful.";
                return RedirectToAction(nameof(LoginConfirmation));
            }
        }
        TempData["Message"] = "Login error.:"+hashedPassword1;
        // Kullanıcı adı veya şifre hatalı
        ModelState.AddModelError(string.Empty, "Invalid username or password.");
    }

    // Eğer ModelState.IsValid false ise veya kimlik doğrulama başarısız olduysa, tekrar login sayfasını göster
    return View(model);
}

// GET: /login/confirmation
public IActionResult LoginConfirmation()
{
    return View();
}


    }
}
