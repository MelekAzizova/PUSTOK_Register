using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WebApplicationPustok.Models;
using WebApplicationPustok.ViewModel.ProductImagesVM;

namespace WebApplicationPustok.ViewModel.ProductVM
{
    public class ProductUpdateVM
    {
        [MaxLength(64)]
        public string Name { get; set; }
        [MaxLength(128)]
        public string Title { get; set; }
        public string Description { get; set; }
        [Column(TypeName = "smallmoney")]
        public decimal SellPrice { get; set; }
        [Column(TypeName = "smallmoney")]
        public decimal CostPrice { get; set; }
        [Range(0, 100)]
        public float Discount { get; set; }
        public ushort Quantity { get; set; }
       
        public string ProductCode { get; set; }
        public IFormFile? ImageFile { get; set; }
        public IEnumerable<IFormFile>? Images { get; set; }
        public IEnumerable<int>? TagIds { get; set; }  
        public IEnumerable<TagProduct> TagProducts { get; set; }
        public IEnumerable<string>? ImageUrls { get; set; }
        /*public IEnumerable<ProductImageVM> ImageUrls { get; set; } = new List<ProductImageVM>();*/

        // public IFormFile Image {  get; set; }
        // public List<ProductImages> productImages { get; set; }
        public int CategoryId { get; set; }
    }
}
