using Binel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Binel.Controllers
{
    public class DonateController : Controller
    {
        readonly BinelProjectContext _context;

        public DonateController(BinelProjectContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdString != null && int.TryParse(userIdString, out int userId))
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
                if (user != null)
                {
                    var organization = await _context.Organizations.FirstOrDefaultAsync(o => o.OrganizationId == user.OrganizationId);
                    if (organization != null)
                    {
                        ViewBag.OrganizationId = organization.OrganizationId;
                        ViewBag.OrganizationName = organization.OrganizationName;
                    }
                    else
                    {
                        return RedirectToAction("Index", "Feed");
                    }
                }
            }
            else
            {
                return BadRequest("Invalid user ID. You must be logged in to the app!!!");
            }

            // Kategorileri ViewBag'e ekleyerek view'e gönder
            ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "CategoryId", "CategoryName");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(DonateCreateViewModel model, int[] Categories,string newcategories)
        {
            if (model == null)
            {
                return BadRequest("Model cannot be null.");
            }

            var donatePost = new DonatePost
            {
                OrganizationId = model.OrganizationId,
                Title = model.Title,
                DonateText = model.DonateText,
                PublishDate = model.PublishDate,
                TotalDonate = model.TotalDonate,
                MaxLimit = model.MaxLimit,
                MinLimit = model.MinLimit,
            };
            if (!string.IsNullOrEmpty(newcategories))
            {
                Category ket=new Category();
                int maxId = _context.Categories.Max(x => x.CategoryId).Value;
                ket.CategoryId = maxId + 1;
                ket.CategoryName=newcategories;
                var returnMdl=_context.Categories.Add(ket);
                _context.SaveChanges();
                donatePost.Categories.Add(returnMdl.Entity);
                
            }

            if (Categories != null)
            {
                foreach (var categoryId in Categories)
                {
                    var category = await _context.Categories.FindAsync(categoryId);
                    if (category != null)
                    {
                        donatePost.Categories.Add(category);
                    }
                }
            }

            if (_context != null)
            {
                await _context.AddAsync(donatePost);
                await _context.SaveChangesAsync();
            }
            else
            {
                return StatusCode(500, "Database context is not initialized.");
            }

            return RedirectToAction("Index", "Feed");
        }
    }
}
