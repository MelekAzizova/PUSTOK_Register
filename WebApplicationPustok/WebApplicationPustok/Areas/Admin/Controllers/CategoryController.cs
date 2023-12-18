using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using WebApplicationPustok.Context;
using WebApplicationPustok.ViewModel.CategoryVM;


namespace Pustok_AzMB.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        PustokDbContext _pd {  get; }

        public CategoryController(PustokDbContext pd)
        {
            _pd = pd;
        }

        public async Task<IActionResult> Index()
        {
           
            var items = await _pd.Categories.Select(c => new CategoryListItemVM
            {
                Name = c.Name,
                Id = c.Id,
                IsDeleted = c.IsDeleted,
                ParentId = c.ParentId,

            }).ToListAsync();
            return View(items);

        }


        public async Task<IActionResult> Create(CategoryCreateVM vm)
        {
          
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            if (await _pd.Categories.AnyAsync(x => x.Name == vm.Name))
            {
                ModelState.AddModelError("Name", "Name already exist ");
                return View(vm);
            }
            await _pd.Categories.AddAsync(new WebApplicationPustok.Models.Category
            {
                Name = vm.Name,
                ParentId = vm.ParentId
            });
            await _pd.SaveChangesAsync();



            return RedirectToAction(nameof(Index));


        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null || id <= 0) return BadRequest();

            var data = await _pd.Categories.FindAsync(id);
            if (data == null) return NotFound();
            return View(new CategoryUpdateVM
            {
                Id=data.Id,
                Name = data.Name,
                IsDeleted=data.IsDeleted,
                ParentId = data.ParentId
                 
            });

        }
        [HttpPost]

        public async Task<IActionResult> Update(int? id, CategoryUpdateVM vm)
        {
            if (id == null || id <= 0) return BadRequest();
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var data = await _pd.Categories.FindAsync(id);
            if (data == null) return NotFound();
            data.Name = vm.Name;
            data.Id= vm.Id;
            data.IsDeleted= vm.IsDeleted;
            data.ParentId = vm.ParentId;


            await _pd.SaveChangesAsync();
            return RedirectToAction(nameof(Index));




        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id==null || id <= 0)
            {
                return BadRequest();
            }
            var data = await _pd.Categories.FindAsync(id);
            if(data==null) return NotFound();
            _pd.Categories.Remove(data);
            await _pd.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        

    }
}
