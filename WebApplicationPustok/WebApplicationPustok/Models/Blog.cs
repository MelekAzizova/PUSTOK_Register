using System.ComponentModel.DataAnnotations;

namespace WebApplicationPustok.Models
{
    public class Blog
    {
        
        public int Id { get; set; }
        [Required,MinLength(3)]
        public string Title { get; set; }
        [Required, MinLength(3)]
        public string Description { get; set; }
        public Author Author { get; set; }
        public int AuthorId { get; set; }
        public DateTime CreatAt { get; set; } 
        public DateTime UpdatAt { get; set; }
        public bool IsDeleted { get; set; }
       
        public ICollection<BlogTag>? BlogTags { get; set;}
        //public IEnumerable<int>? TagIds { get; set; }
        
    }
}
