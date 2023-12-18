using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.Intrinsics.Arm;
using WebApplicationPustok.Context;
using WebApplicationPustok.Models;
using WebApplicationPustok.ViewModel.AuthorVM;
using WebApplicationPustok.ViewModel.TagVM;

namespace WebApplicationPustok.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AuthorController : Controller
    {
        public AuthorController(PustokDbContext db)
        {
            _db = db;
        }

        PustokDbContext _db {  get; set; }
        public async Task< IActionResult> Index()
        {
            var items = await _db.Authors.Select(s => new AuthorListItemVM
            {
                Id = s.Id,
                Name = s.Name,
                Surname = s.Surname
            }).ToListAsync();
            return View(items);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Create(AuthorCreateVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }


            Author author = new Author
            {
                Name = vm.Name,
                Surname = vm.Surname
            };
            await _db.Authors.AddAsync(author);
            await _db.SaveChangesAsync();
            return View();
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null || id <= 0) return BadRequest();

            var data = await _db.Authors.FindAsync(id);
            if (data == null) return NotFound();
            return View(new AuthorUpdateVM
            {
                Name=data.Name,
                Surname=data.Surname

            });

        }
        [HttpPost]

        public async Task<IActionResult> Update(int? id, AuthorUpdateVM vm)
        {
            if (id == null || id <= 0) return BadRequest();
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var data = await _db.Authors.FindAsync(id);
            if (data == null) return NotFound();
            data.Name=vm.Name;
            data.Surname=vm.Surname;

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));




        }
        public async Task<IActionResult> Delete(int? id)
        {

            if (id == null) return BadRequest();

            var data = await _db.Authors.FindAsync(id);
            if (data == null) return NotFound();
            _db.Authors.Remove(data);
            await _db.SaveChangesAsync();
            
            return RedirectToAction(nameof(Index));

        }
    }
}
