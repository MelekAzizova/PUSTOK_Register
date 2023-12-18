namespace WebApplicationPustok.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public int? ParentId { get; set; }
        public IEnumerable<Product>? Products { get; set; }
    }
}
