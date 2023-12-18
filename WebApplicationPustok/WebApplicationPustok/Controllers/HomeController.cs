using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationPustok.Areas.Admin.ViewModels;
using WebApplicationPustok.Context;
using WebApplicationPustok.ViewModel.HomeVM;
using WebApplicationPustok.ViewModel.ProductVM;
using WebApplicationPustok.ViewModel.SliderVM;

namespace WebApplicationPustok.Controllers
{
    public class HomeController : Controller
    {
        public HomeController(PustokDbContext pd)
        {
            _pd = pd;
        }

        PustokDbContext _pd {  get; }
        
        
        public async Task<IActionResult> Index()
        {

            HomeVM vm = new HomeVM
            {
                Sliders = await _pd.Sliders.Select(s => new SliderListItemVM
                {
                    Id = s.Id,
                    ImageUrl = s.ImageUrl,
                    IsLeft = s.IsLeft,
                    Title = s.Title,
                    Description = s.Description
                }).ToListAsync(),

                 Products=await _pd.Products.Where(p=> !p.IsDeleted).Select(p=> new ProductListItem
                 {
                     Id = p.Id,
                     Name   = p.Name,
                     Title= p.Title,
                     Description = p.Description,
                     SellPrice= p.SellPrice,
                     CostPrice= p.CostPrice,
                     Discount= p.Discount,
                     Quantity= p.Quantity,
                     ProductCode= p.ProductCode,
                     Category= p.Category,
                     Image=p.ImagrUrl
                     
                 }).ToListAsync()
            };

            return View(vm);
        }

        //public async Task<IActionResult> ProductPagination(int page = 1, int count = 8)
        //{
        //    var items = _pd.Products.Where(p => !p.IsDeleted).Skip((page - 1) * count).Take(count).Select(p => new AdminProductListItemVM
        //    {
               
        //    });
        //    return Json(items);
        //}

    }
}
