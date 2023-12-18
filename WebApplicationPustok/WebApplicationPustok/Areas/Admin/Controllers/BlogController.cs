using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationPustok.Context;
using WebApplicationPustok.Models;
using WebApplicationPustok.ViewModel.BlogVM;

namespace WebApplicationPustok.Areas.Admin.Controllers
{
        [Area("Admin")]
    public class BlogController : Controller
    {
        public BlogController(PustokDbContext db)
        {
            _db = db;
        }

        PustokDbContext _db {  get; set; }
        public async  Task<IActionResult> Index()
        {
            var items = await _db.Blogs.Select(s => new BlogListItem
            {
                Id = s.Id,
                Title = s.Title,
                Description = s.Description,
                AuthorId = s.AuthorId,
                CreatAt = DateTime.Now,
                UpdatAt = DateTime.Now,
                IsDeleted = false,
                Blogs=s.BlogTags.Select(s=>s.Blog).ToList(),
                
                
            }).ToListAsync();
            return View(items);
            

        }
        public IActionResult Create()
        {
            ViewBag.Tags = _db.Tags.ToList();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(BlogCreateVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            if (!ModelState.IsValid)
            {
                ViewBag.Tags = _db.Tags;
              

                return View(vm);
            }
            if (await _db.Tags.Where(c => vm.TagIds.Contains(c.Id)).Select(c => c.Id).CountAsync() != vm.TagIds.Count())
            {
                ModelState.AddModelError("ColorIds", "Color doesnt exist");
                ViewBag.Tags = _db.Tags;

                return View(vm);
            }
            Blog blog = new Blog
            {
                Title = vm.Title,
                Description = vm.Description,
                AuthorId=vm.AuthorId,
                CreatAt=DateTime.Now,
                
                IsDeleted=false
            };
            await _db.Blogs.AddAsync(blog);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null || id <= 0) return BadRequest();

            var data = await _db.Blogs.FindAsync(id);
            if (data == null) return NotFound();
            return View(new BlogUpdateVM
            {
                Title=data.Title,
                Description=data.Description,
                AuthorId=data.AuthorId,
                UpdatAt=DateTime.Now,
                IsDeleted=false


            });

        }
        [HttpPost]

        public async Task<IActionResult> Update(int? id, BlogUpdateVM vm)
        {
            if (id == null || id <= 0) return BadRequest();
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var data = await _db.Blogs.FindAsync(id);
            if (data == null) return NotFound();
            
            data.Title = vm.Title;
            data.Description = vm.Description;
            data.AuthorId = vm.AuthorId;
            data.UpdatAt = DateTime.Now;
            data.IsDeleted = false;
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));




        }
        public async Task<IActionResult> Delete(int? id)
        {

            if (id == null) return BadRequest();

            var data = await _db.Blogs.FindAsync(id);
            if (data == null) return NotFound();
            _db.Blogs.Remove(data);
            await _db.SaveChangesAsync();
           
            return RedirectToAction(nameof(Index));

        }
    }
}
