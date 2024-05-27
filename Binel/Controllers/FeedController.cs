using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Binel.Models;

namespace Binel.Controllers
{
    public class FeedController : Controller
    {
        private readonly BinelProjectContext _context;

        public FeedController(BinelProjectContext context)
        {
            _context = context;
        }

        // GET: /feed
        [Authorize] // Sadece yetkilendirilmiş kullanıcılar erişebilir
        public IActionResult Index(string keyword, int pageNum = 1)
        {
            return RedirectToAction("Search", new { keyword = keyword, pageNum = pageNum });
        }

        // GET: /feed/search?keyword=[Arama Kelimesi]&pageNum=[Sayfa Numarası]
        [Authorize] // Sadece yetkilendirilmiş kullanıcılar erişebilir
        public IActionResult Search(string keyword, int pageNum = 1)
        {
            IQueryable<DonatePost> query = _context.DonatePosts
                .Include(p => p.Organization)
                .Include(p => p.Categories);

            if (!string.IsNullOrEmpty(keyword))
            {
                // Case-insensitive arama için
                query = query.Where(p =>
                    (p.Title != null && EF.Functions.Like(p.Title, $"%{keyword}%")) ||
                    (p.DonateText != null && EF.Functions.Like(p.DonateText, $"%{keyword}%")) ||
                    (p.Organization != null && EF.Functions.Like(p.Organization.OrganizationName, $"%{keyword}%")) ||
                    (p.Categories.Any(c => c.CategoryName != null && EF.Functions.Like(c.CategoryName, $"%{keyword}%")))
                );
            }

            var donatePosts = query.OrderByDescending(p => p.PublishDate)
                                   .Skip((pageNum - 1) * 10)
                                   .Take(10)
                                   .ToList();

            int totalPostCount = query.Count();
            int totalPages = (int)Math.Ceiling((double)totalPostCount / 10);

            ViewBag.keyword = keyword;
            ViewBag.TotalPages = totalPages;

            return View("Feed", donatePosts);
        }
    }
}
