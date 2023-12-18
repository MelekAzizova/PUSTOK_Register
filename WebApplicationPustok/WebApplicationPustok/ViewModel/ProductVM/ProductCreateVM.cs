
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WebApplicationPustok.Models;

namespace WebApplicationPustok.ViewModel.ProductVM
{
    public class ProductCreateVM
    {
      
        [MaxLength(64)]
        public string Name { get; set; }
        [MaxLength(128)]
        public string? Title { get; set; }
        public string? Description { get; set; }
        [Column(TypeName = "smallmoney")]
        public decimal SellPrice { get; set; }
        [Column(TypeName = "smallmoney")]
        public decimal CostPrice { get; set; }
        [Range(0, 100)]
        public float Discount { get; set; }
        public ushort Quantity { get; set; }
        public string ProductCode { get; set; }
        public bool IsDeleted { get; set; } = false;
        public IFormFile ImgFile { get; set; }
        public IEnumerable<IFormFile>  Images { get; set; }
        public int CategoryId { get; set; }

        public IEnumerable<int>? TagIds {  get; set; }
    }
}
