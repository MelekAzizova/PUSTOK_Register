using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WebApplicationPustok.Models;

namespace WebApplicationPustok.ViewModel.ProductVM
{
    public class ProductListItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        public string? Title { get; set; }
        public string? Description { get; set; }
       
        public decimal SellPrice { get; set; }
       
        public decimal CostPrice { get; set; }
       
        public float Discount { get; set; }
        public ushort Quantity { get; set; }

        public string ProductCode { get; set; }
        public string Image { get; set; }

        public int ProductId { get; set; }
       // public List<ProductImages>? productImages { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
