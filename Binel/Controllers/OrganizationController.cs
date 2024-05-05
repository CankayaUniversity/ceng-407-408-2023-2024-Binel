using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
