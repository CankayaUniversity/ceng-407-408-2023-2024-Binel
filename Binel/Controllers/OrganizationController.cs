using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Binel.Models;

namespace Binel.Controllers
{
    public class OrganizationController : Controller
    {
        private readonly BinelProjectContext _context;

        public OrganizationController(BinelProjectContext context)
        {
            _context = context;
        }

        // Organizasyon profili sayfası
        public async Task<IActionResult> Profile(string organizationTitle)
        {
            var organization = await _context.Organizations
                .Include(o => o.DonatePosts)
                    .ThenInclude(dp => dp.Files)
                .Include(o => o.Posts)
                    .ThenInclude(p => p.Files)
                .FirstOrDefaultAsync(o => o.OrganizationTitle == organizationTitle);

            if (organization == null)
            {
                return NotFound();
            }

            return View(organization);
        }

        // Bağış gönderisi detay sayfası
        [Route("{organizationTitle}/donatepost")]
        public async Task<IActionResult> DonatePost(int? id, string organizationTitle)
        {
            if (id == null || string.IsNullOrEmpty(organizationTitle))
            {
                return NotFound();
            }

            var donatePost = await _context.DonatePosts
                .Include(dp => dp.Files)
                .FirstOrDefaultAsync(dp => dp.DonateId == id && dp.Organization.OrganizationTitle == organizationTitle);

            if (donatePost == null)
            {
                return NotFound();
            }

            return View(donatePost);
        }

        // Bağış yapma işlemi
        [HttpPost]
        public async Task<IActionResult> Donate(int id, int donateAmount)
        {
            // Kullanıcının ID'sini al
            var userId = HttpContext.Session.GetInt32("UserID");

            // Kullanıcı oturumu kontrolü
            if (userId == null)
            {
                // Kullanıcı oturumu yoksa, giriş yapmaları için yönlendir
                return RedirectToAction("Login", "Users");
            }

            var donatePost = await _context.DonatePosts
                .Include(dp => dp.Organization) // Organization navigasyon özelliğini dahil et
                .FirstOrDefaultAsync(dp => dp.DonateId == id);

            if (donatePost == null)
            {
                return NotFound();
            }

            // Bağış miktarını kontrol et
            if (donateAmount < donatePost.MinLimit || donateAmount > donatePost.MaxLimit)
            {
                // Hata durumunda donatePost'u tekrar gönder ve hata mesajını TempData'ye ekle
                TempData["ErrorMessage"] = "Invalid donate amount";
                return RedirectToAction("DonatePost", new { id = id, organizationTitle = donatePost.Organization.OrganizationTitle });
            }

            // Toplam bağış miktarını güncelle
            donatePost.TotalDonate = (donatePost.TotalDonate ?? 0) + donateAmount;

            // Bağış logunu oluştur
            var donateLog = new DonateLog
            {
                DonateId = donatePost.DonateId,
                DonateDate = DateTime.Now,
                Amount = donateAmount,
                Users = new List<User> { await _context.Users.FindAsync(userId) } // Kullanıcıyı bağış loguna ekle
            };

            // Logu veritabanına ekle
            _context.DonateLogs.Add(donateLog);

            // Veritabanında değişiklikleri kaydet
            await _context.SaveChangesAsync();

            // Başarılı bağış durumunda başarı sayfasına yönlendirme yap
            ViewBag.DonateTitle = donatePost.Title;
            return View("DonateSuccess");
        }


        // Normal gönderi detay sayfası
        [Route("{organizationTitle}/post")]
        public async Task<IActionResult> Post(int? id, string organizationTitle)
        {
            if (id == null || string.IsNullOrEmpty(organizationTitle))
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.Files)
                .FirstOrDefaultAsync(p => p.PostId == id && p.Organization.OrganizationTitle == organizationTitle);

            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }
    }
}
