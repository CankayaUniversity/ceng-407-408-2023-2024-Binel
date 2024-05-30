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
    public async Task<IActionResult> Index()
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdString != null && int.TryParse(userIdString, out int userId))
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
                if (user != null)
                {
                    var organizations = await _context.DonatePosts.Where(o => o.OrganizationId == user.OrganizationId).ToListAsync();
                    return View(organizations);
                }
            }

            return BadRequest();

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
                Files = new List<FileUrl>()

            };
                if (model.UploadedFilesDonate != null)
                    {
                foreach (var file in model.UploadedFilesDonate)
                {
                    if (file.Length > 0)
                    {

                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;


                        var fileUrl = new FileUrl
                        {
                            FileUrl1 = uniqueFileName
                        };
                        donatePost.Files.Add(fileUrl);

                    }
                }
            }


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
         public async Task<IActionResult> Delete(int id)
        {
            var donatePost = await _context.DonatePosts.FindAsync(id);
            return View(donatePost);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(DonatePost donateposts)
        {
             _context.Database.ExecuteSqlRaw("Delete from Donate_Categories where donate_ID = "+donateposts.DonateId);
            _context.Database.ExecuteSqlRaw("Delete from Media_Donate_Files where donate_ID = "+donateposts.DonateId);

            _context.DonatePosts.Remove(donateposts);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
       
    }
}
