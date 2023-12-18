
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WebApplicationPustok.Models;

namespace WebApplicationPustok.Areas.Admin.ViewModels
{
    public class AdminProductListItemVM
    {
        public int Id { get; set; }
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

       
        public string ImgFile { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public bool IsDeleted { get; set; } = false;
        public Tag Tag { get; set; }
        public IEnumerable<Tag> Tags { get; set; } 
    }
}
