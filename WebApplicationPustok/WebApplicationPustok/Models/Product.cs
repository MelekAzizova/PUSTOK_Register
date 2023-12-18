using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApplicationPustok.Models
{
    public class Product
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
        public string ImagrUrl{  get; set; }
       
        public int  ProductId { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public bool IsDeleted { get; set; } = false;
        public int TagIds { get; set; }
        public ICollection<TagProduct>? TagProducts { get; set; }
        public ICollection<ProductImages>? ProductImages { get; set; } 
    }
}
