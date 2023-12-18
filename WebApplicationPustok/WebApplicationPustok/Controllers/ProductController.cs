using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebApplicationPustok.Areas.Admin.ViewModels;
using WebApplicationPustok.Context;
using WebApplicationPustok.Helpers;
using WebApplicationPustok.ViewModel.ProductVM;
using WebApplicationPustok.ViewModel.ShopingCard;

namespace WebApplicationPustok.Controllers
{
    public class ProductController : Controller
    {
        public ProductController(PustokDbContext db)
        {
            _db = db;
        }

        PustokDbContext _db {  get;  }
        public async Task<IActionResult> Index()
        {
            //var products =  _db.Products.Include(i => i.productImages).ToList();
            AdminProductListItemVM items = new AdminProductListItemVM();

            var x = await _db.Products.Select(p => new AdminProductListItemVM
            {
                Id = p.Id,
                Name = p.Name,
                Title = p.Title,
                Description = p.Description,
                CostPrice = p.CostPrice,
                Discount = p.Discount,
                Category = p.Category,

                IsDeleted = p.IsDeleted,
                Quantity = p.Quantity,
                SellPrice = p.SellPrice,
                ProductCode = p.ProductCode,
                ImgFile = p.ImagrUrl,
                CategoryId = p.CategoryId



            }).ToListAsync();

            return View(x);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || id <= 0) return BadRequest();
            var data = await _db.Products.Select( s=> new ProductListVM
            {
                Id = s.Id,
                Name = s.Name,
                Title = s.Title,
                Description = s.Description,
                SellPrice = (s.SellPrice - (s.SellPrice * (decimal)s.Discount) / 100),
                Discount = s.Discount,
                Quantity = s.Quantity,
                ProductCode = s.ProductCode,
                IsDeleted = s.IsDeleted,
                Category = s.Category,
                ImageUrls=s.ProductImages.Select(s=>s.ImageUrl).ToList(),
                ImgFile =s.ImagrUrl

            }). SingleOrDefaultAsync(p => p.Id == id);
            if (data == null) return NotFound();
            return View(data);
            
        }
        public async Task<IActionResult> AddShopingCard(int? id)
        {
            if (id == null || id <= 0) return BadRequest();
            if (!await _db.Products.AnyAsync(p => p.Id == id)) return NotFound();
            var basket = JsonConvert.DeserializeObject<List<ShopingCardCountAndProductVM>>(HttpContext.Request.Cookies["basket"] ?? "[]");
            var existItem = basket.Find(b => b.Id == id);
            if (existItem == null)
            {
                basket.Add(new ShopingCardCountAndProductVM
                {
                    Id = (int)id,
                    Count = 1
                });
            }
            else
            {
                existItem.Count++;
            }
            HttpContext.Response.Cookies.Append("basket", JsonConvert.SerializeObject(basket), new CookieOptions
            {
                MaxAge = TimeSpan.MaxValue
            });
            return Ok();
        }
        public IActionResult GetShopingCard()
        {
            return ViewComponent("ShopingCard");
        }
    }
}
