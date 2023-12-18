using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using WebApplicationPustok.Areas.Admin.ViewModels;
using WebApplicationPustok.Context;
using WebApplicationPustok.Helpers;
using WebApplicationPustok.Models;
using WebApplicationPustok.ViewModel.ProductImagesVM;
using WebApplicationPustok.ViewModel.ProductVM;


namespace WebApplicationPustok.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
         PustokDbContext _db;
         IWebHostEnvironment _env;
        public ProductController(PustokDbContext dbContext, IWebHostEnvironment env = null)
        {
            _db = dbContext;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            //var products =  _db.Products.Include(i => i.productImages).ToList();
            AdminProductListItemVM items = new AdminProductListItemVM();

            var item = await _db.Products.Select(p => new AdminProductListItemVM
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
                CategoryId = p.CategoryId,
                Tags = p.TagProducts.Select(pc => pc.Tag)
                }).ToListAsync();
           
            return View(item);
        }
           
            public IActionResult Create()
            {

                // ViewBag.Categories = _db.Categories;
                ViewBag.Tags=_db.Tags.ToList();

                ViewBag.Categories = _db.Categories.ToList();
                return View();
            }

        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateVM vm)     
        {

            if (vm.ImgFile != null)
            {
                if (!vm.ImgFile.IsCorrectType())
                {
                    ModelState.AddModelError("ImageFile", "Wrong file type");
                }
                if (!vm.ImgFile.IsValidSize(20f))
                {
                    ModelState.AddModelError("ImgFile", "Wrong file size");
                }
            }
            if (!ModelState.IsValid)
            {
                ViewBag.Tags = _db.Tags.ToList();
                ViewBag.Categories = _db.Categories.ToList();
               
                return View(vm);
            }
            if (await _db.Products.AnyAsync(x => x.Name == vm.Name))
            {
                ModelState.AddModelError("Name", "Name already exist");

                return View(vm);
            }
           
            //images null deyilse ve count 0dan boyukdeuse
            if (vm.Images?.Count() > 0)
            {
               
                foreach(var img in vm.Images)
                {
                    if (!img.IsCorrectType())
                    {
                        ModelState.AddModelError("Image", "Wrong file type("+ img.FileName+")");
                    }
                    if (!img.IsValidSize(20f))
                    {
                        ModelState.AddModelError("Image", "Wrong file size(" + img.FileName + ")");
                    }
                    

                }
            }
            if (await _db.Tags.Where(c => vm.TagIds.Contains(c.Id)).Select(c => c.Id).CountAsync() != vm.TagIds.Count())
            {
                ModelState.AddModelError("TagsId", "Tag doesnt exist");
                ViewBag.Categories = _db.Categories;
                ViewBag.Tags = new SelectList(_db.Tags, "Id", "Title");
                return View(vm);
            }

            Product product = new Product
            {
                Name = vm.Name,
                Title = vm.Title,
                Description = vm.Description,
                CostPrice = vm.CostPrice,
                Discount = vm.Discount,

                Quantity = vm.Quantity,
                SellPrice = vm.SellPrice,
                ProductCode = vm.ProductCode,

                CategoryId = vm.CategoryId,

                TagProducts=vm.TagIds.Select(id=>new TagProduct
                {
                    TagId = id
                }).ToList(),
                ImagrUrl = await vm.ImgFile.SaveAsync(PathConstants.Product),
                ProductImages = vm.Images.Select( s => new ProductImages
                {
                    ImageUrl =  s.SaveAsync(PathConstants.Product).Result
                }).ToList()
            };
           
            await _db.Products.AddAsync(product);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //public async Task<IActionResult> Update(int? id, ProductListItem vm)
        //{


        //    if (id == null || id <= 0) return BadRequest();

        //    var data = await _db.Products.FindAsync(id);
        //    if (data == null) return NotFound();
        //    return View(new ProductUpdateVM
        //    {
        //        Name = data.Name,
        //        Title = data.Title,
        //        Description = data.Description,
        //        CostPrice = data.CostPrice,
        //        Discount = data.Discount,
        //        Quantity = data.Quantity,
        //        SellPrice = data.SellPrice,
        //        ProductCode = data.ProductCode,

        //        CategoryId = data.CategoryId

        //    });

        //}
        //[HttpPost]

        //public async Task<IActionResult> Update(int? id, ProductUpdateVM vm)
        //{
        //    if (id == null || id <= 0) return BadRequest();
        //    if (!ModelState.IsValid)
        //    {
        //        return View(vm);
        //    }

        //    var data = await _db.Products.FindAsync(id);
        //    if (data == null) return NotFound();
        //    data.Name = vm.Name;
        //    data.Title = vm.Title;
        //    data.Description = vm.Description;
        //    data.CostPrice = vm.CostPrice;
        //    data.Discount = vm.Discount;

        //    data.Quantity = vm.Quantity;
        //    data.SellPrice = vm.SellPrice;
        //    data.ProductCode = vm.ProductCode;

        //    data.CategoryId = vm.CategoryId;


        //    await _db.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));




        //}

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null || id <= 0) return BadRequest();
            
            ViewBag.Tags = new SelectList(_db.Tags, nameof(Tag.Id), nameof(Tag.Title));
            ViewBag.Categories = new SelectList(_db.Categories, nameof(Category.Id), nameof(Category.Name));
            var data = await _db.Products
                .Include(p => p.ProductImages)

                .SingleOrDefaultAsync(p => p.Id == id);


            if (data == null) return NotFound();

            var vm = new ProductUpdateVM
            {
                CategoryId = data.CategoryId,

                TagIds = data.TagProducts?.Select(s => s.TagId),
                ProductCode = data.ProductCode,
                Title = data.Title,
                CostPrice = data.CostPrice,
                Description = data.Description,
                Discount = data.Discount,
                Name = data.Name,
                Quantity = data.Quantity,
                SellPrice = data.SellPrice,

               
                
              

                
                //CoverImageUrl = data.ImageUrl
            };

            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id, ProductUpdateVM vm)
        {
            if (id == null || id <= 0) return BadRequest();
            if (vm.ImageFile != null)
            {
                if (!vm.ImageFile.IsCorrectType())
                {
                    ModelState.AddModelError("ImageFile", "Wrong file type");
                }
                if (!vm.ImageFile.IsValidSize())
                {
                    ModelState.AddModelError("ImageFile", "Files length must be less than kb");
                }
            }
            if (vm.Images != null)
            {
                //string message = string.Empty;
                foreach (var img in vm.Images)
                {
                    if (!img.IsCorrectType())
                    {
                        ModelState.AddModelError("", "Wrong file type (" + img.FileName + ")");
                        //message += "Wrong file type (" + img.FileName + ") \r\n";
                    }
                    if (!img.IsValidSize(200))
                    {
                        ModelState.AddModelError("", "Files length must be less than kb (" + img.FileName + ")");
                        //message += "Files length must be less than kb (" + img.FileName + ") \r\n";
                    }
                }
            }
            if (vm.CostPrice > vm.SellPrice)
            {
                ModelState.AddModelError("CostPrice", "Sell price must be bigger than cost price");
            }

            if (!vm.TagIds.Any())
            {
                ModelState.AddModelError("TagIds", "You must add at least 1 tag");
            }
            if (!ModelState.IsValid)
            {

                ViewBag.Categories = new SelectList(_db.Categories, nameof(Category.Id), nameof(Category.Name));
                return View(vm);
            }

            var data = await _db.Products
                .Include(p => p.ProductImages)

                .SingleOrDefaultAsync(p => p.Id == id);
            if (data == null) return NotFound();

            if (vm.Images != null)
            {
                var imgs = vm.Images.Select(i => new ProductImages
                {
                    ImageUrl = i.SaveAsync(PathConstants.Product).Result,
                    ProductId = data.Id
                });

                data.ProductImages.AddRange(imgs);
            }


            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Delete(int? id)
        {

            if (id == null) return BadRequest();

            var data = await _db.Products.FindAsync(id);
            if (data == null) return NotFound();
            _db.Products.Remove(data);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> ProductPagination(int page=1,int count = 4)
        {
            var data = await _db.Products.Where(p => !p.IsDeleted).Skip((page-1)*count).Take(count).ToListAsync();
            return Json(data);
        }
    }

}

