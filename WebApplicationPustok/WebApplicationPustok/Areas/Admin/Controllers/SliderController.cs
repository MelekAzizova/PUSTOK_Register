using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using WebApplicationPustok.Context;
using WebApplicationPustok.Helpers;
using WebApplicationPustok.Models;
using WebApplicationPustok.ViewModel.CategoryVM;
using WebApplicationPustok.ViewModel.SliderVM;


namespace WebApplicationPustok.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SliderController : Controller
    {
        PustokDbContext _pd {  get;  }

        public SliderController(PustokDbContext pd)
        {
            _pd = pd;
        }

        public async Task<IActionResult> Index()
        {
           

                var items = await _pd.Sliders.Select(s => new SliderListItemVM
                {
                    Title = s.Title,
                    Description = s.Description,
                    ImageUrl = s.ImageUrl,
                    IsLeft = s.IsLeft,
                   
                    Id = s.Id
                }).ToListAsync();
                return View(items);
            

        }

        public IActionResult Create()
        {

            return View();

        }

        [HttpPost]

        public async Task<IActionResult> Create(SliderCreateVM vm)
        {
            if (vm.ImgFile != null)
            {
                if (!vm.ImgFile.IsCorrectType())
                {
                    ModelState.AddModelError("ImageFile", "Wrong file type");
                }
                if (!vm.ImgFile.IsValidSize(100f))
                {
                    ModelState.AddModelError("ImgFile", "Wrong file size");
                }
            }
            if (vm.Position < -1 || vm.Position > 1)
            {
                ModelState.AddModelError("Position", "Wrong input");
            }

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

           
            Slider slider = new Slider
            {
                Title = vm.Title,
                Description = vm.Description,
                ImageUrl = await vm.ImgFile.SaveAsync(PathConstants.Product),
                IsLeft = vm.Position switch
                {
                    1 => true,
                    0 => false
                }


            };
            await _pd.Sliders.AddAsync(slider);
            await _pd.SaveChangesAsync();
            return RedirectToAction(nameof(Index));


        }
        public async Task<IActionResult> Delete(int? id)
        {
            
            if (id == null) return BadRequest();
            
            var data = await _pd.Sliders.FindAsync(id);
            if (data == null) return NotFound();
            _pd.Sliders.Remove(data);
            await _pd.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null || id<=0) return BadRequest();
            
            var data = await _pd.Sliders.FindAsync(id);
            if(data==null) return NotFound();
            
            return View(new SliderUpdateVM
            {
               
                Title=data.Title,
                Description=data.Description,
                ImageUrl=data.ImageUrl,
                Position=data.IsLeft switch
                {
                     true=>1,
                     false=>0
                }
            });

        }
        [HttpPost]

        public async Task<IActionResult> Update(int? id, SliderUpdateVM vm)
        {
            if (id == null || id <= 0) return BadRequest();
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var data = await _pd.Sliders.FindAsync(id);
            if (data == null) return NotFound();
            data.Title = vm.Title;
            data.Description = vm.Description;
            data.ImageUrl = vm.ImageUrl;
            data.IsLeft = vm.Position switch
            {
                1 => true,
                0 => false
            };
            await _pd.SaveChangesAsync();
            return RedirectToAction(nameof(Index));




        }
       
    }

    

       






        //public async Task<IActionResult> Create(Slider slider)
        //{
        //    if(!ModelState.IsValid)
        //    {
        //        return View(slider);
        //    }
        //    using PustokDbContext context = new PustokDbContext();
        //    await context.Sliders.AddAsync(slider);
        //    await context.SaveChangesAsync(); 


        //    return View();


        //}

    }


