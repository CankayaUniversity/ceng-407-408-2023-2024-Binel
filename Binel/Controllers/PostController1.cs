using Binel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Binel.Controllers
{
    public class PostController : Controller
    {
        readonly BinelProjectContext _context;

        public PostController(BinelProjectContext context)
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
                    var organizations = await _context.Posts.Where(o => o.OrganizationId == user.OrganizationId).ToListAsync();
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

                return BadRequest("Invalid user ID. You must be login to app!!!");
            }


            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(PostCreateViewModel model)
        {
            if (model == null)
            {
                return BadRequest("Model cannot be null.");
            }

            var post = new Post
            {
                OrganizationId = model.OrganizationId,
                Title = model.Title,
                PostText = model.PostText,
                PublishDate = model.PublishDate,
                ExternalPlatform = model.ExternalPlatform,
                Files = new List<FileUrl>()
            };

            if (model.UploadedFiles != null)
            {
                foreach (var file in model.UploadedFiles)
                {
                    if (file.Length > 0)
                    {

                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;


                        var fileUrl = new FileUrl
                        {
                            FileUrl1 = uniqueFileName
                        };
                        post.Files.Add(fileUrl);

                    }
                }
            }

            if (_context != null)
            {
                await _context.AddAsync(post);
                await _context.SaveChangesAsync();
            }
            else
            {
                return StatusCode(500, "Database context is not initialized.");
            }

            return RedirectToAction("Index");
        }
       
        public async Task<IActionResult> Delete(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            return View(post);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(Post post)
        {
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

    }
}
