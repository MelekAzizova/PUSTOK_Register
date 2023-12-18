using WebApplicationPustok.Models;

namespace WebApplicationPustok.ViewModel.BlogVM
{
    public class BlogListItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Author Author { get; set; }
        public int AuthorId { get; set; }
        public DateTime CreatAt { get; set; }
        public DateTime UpdatAt { get; set; }
        public bool IsDeleted { get; set; }
        public IEnumerable<Blog>? Blogs { get; set; }    
        public IEnumerable<int>? TagIds { get; set; }

    }
}
