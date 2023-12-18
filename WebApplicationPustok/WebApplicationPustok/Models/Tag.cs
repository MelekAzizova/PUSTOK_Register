using System.Data;

namespace WebApplicationPustok.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Title { get; set; }
       
        public ICollection<TagProduct>? TagProducts { get; set; }
        
    }
}
