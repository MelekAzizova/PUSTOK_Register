using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationPustok.Context;
using WebApplicationPustok.Helpers;
using WebApplicationPustok.Models;
using WebApplicationPustok.ViewModel.ProductVM;

namespace WebApplicationPustok.ViewComponents
{
    public class ProductViewComponent : ViewComponent
    {
        public ProductViewComponent(PustokDbContext db)
        {
            _db = db;
        }

        PustokDbContext _db { get; }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await _db.Products.Select(s=>new ProductListItem
            {
                Id = s.Id,
                Name = s.Name,
                Title = s.Title,
                Description = s.Description,
                SellPrice = s.SellPrice,
                Category = s.Category,
                CostPrice = s.CostPrice,
                Discount = s.Discount,
                Quantity = s.Quantity,
                ProductCode = s.ProductCode,
                IsDeleted = s.IsDeleted,
                Image=s.ImagrUrl
                

            }).ToListAsync());
        }
    }
}
