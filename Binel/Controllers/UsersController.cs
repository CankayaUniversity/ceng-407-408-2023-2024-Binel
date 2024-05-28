﻿using Microsoft.AspNetCore.Mvc;
using Binel.Models;
using Binel.ViewModels;
using System.Security.Cryptography;
using System.Text;
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

        #region Duygu start
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
        #endregion

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
[ValidateAntiForgeryToken]
public async Task<IActionResult> Login(LoginViewModel model)
{
    if (ModelState.IsValid)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
        if (user != null)
        {
            string hashedPassword = ComputeSHA256Hash(model.Password);
            if (user.PasswordHash == hashedPassword)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString())
                };
                var identity = new ClaimsIdentity(claims, "login");
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(principal);
                
                // Kullanıcı türünü belirle ve Session'a kaydet
                if (user.OrganizationId == null)
                {
                    // Bireysel kullanıcı olarak işaretle
                    HttpContext.Session.SetString("UserType", "Individual");
                }
                else
                {
                    // Kurumsal kullanıcı olarak işaretle
                    HttpContext.Session.SetString("UserType", "Corporate");
                }
                
                // Session'a kullanıcı ID'sini kaydet
                HttpContext.Session.SetInt32("UserID", user.UserId);
                
                TempData["Message"] = "Login successful.";
                return RedirectToAction(nameof(LoginConfirmation));
            }
            else
            {
                TempData["Message"] = "Login error: Invalid username or password.";
            }
        }
        else
        {
            TempData["Message"] = "Login error: Account not found.";
        }
    }
    // ModelState.IsValid false ise, buraya ulaşılması, formun doğrulanamaması anlamına gelir.
    // Bu durumda ModelState zaten "Invalid username or password." hatası verecek.
    return View(model);
}
        // GET: /login/confirmation
        public IActionResult LoginConfirmation()
        {
            return View();
        }
        // POST: /logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();

            HttpContext.Session.Clear();

            TempData["Message"] = "Logout successful.";

            return RedirectToAction("Login", "Users");
        }

    }
}
