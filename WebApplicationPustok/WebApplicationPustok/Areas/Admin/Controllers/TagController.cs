using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationPustok.Context;
using WebApplicationPustok.Helpers;
using WebApplicationPustok.Models;
using WebApplicationPustok.ViewModel.SliderVM;
using WebApplicationPustok.ViewModel.TagVM;

namespace WebApplicationPustok.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TagController : Controller
    {
        public TagController(PustokDbContext dp)
        {
            _dp = dp;
        }

        PustokDbContext _dp {  get; set; }  

        public async  Task<IActionResult> Index()
        {
            var items = await _dp.Tags.Select(s => new TagListItem
            {
                Id = s.Id,
                Title = s.Title,
               
                

            }).ToListAsync();
            return View(items);
           
        }
        public IActionResult Create()
        {

            return View();

        }

        [HttpPost]

        public async Task<IActionResult> Create(TagCreateVM vm)
        {
            

            if (!ModelState.IsValid)
            {
                return View(vm);
            }


            Tag tag = new Tag
            {
                Title = vm.Title,
              


            };
            await _dp.Tags.AddAsync(tag);
            await _dp.SaveChangesAsync();
            return RedirectToAction(nameof(Index));


        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null || id <= 0) return BadRequest();

            var data = await _dp.Tags.FindAsync(id);
            if (data == null) return NotFound();
            return View(new TagUpdateVM
            {
                Id=data.Id,
                Title = data.Title,
               

            });

        }
        [HttpPost]

        public async Task<IActionResult> Update(int? id, TagUpdateVM vm)
        {
            if (id == null || id <= 0) return BadRequest();
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var data = await _dp.Tags.FindAsync(id);
            if (data == null) return NotFound();
            data.Id = vm.Id;
            data.Title = vm.Title;
           
            await _dp.SaveChangesAsync();
            return RedirectToAction(nameof(Index));




        }
        public async Task<IActionResult> Delete(int? id)
        {

            if (id == null) return BadRequest();

            var data = await _dp.Tags.FindAsync(id);
            if (data == null) return NotFound();
            _dp.Tags.Remove(data);
            await _dp.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
    }
}
