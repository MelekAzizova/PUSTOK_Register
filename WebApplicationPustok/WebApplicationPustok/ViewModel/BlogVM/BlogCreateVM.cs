using WebApplicationPustok.Models;

namespace WebApplicationPustok.ViewModel.BlogVM
{
    public class BlogCreateVM
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Author? Author { get; set; }
        public int AuthorId { get; set; }
        public DateTime CreatAt { get; set; }= DateTime.Now;

        public bool IsDeleted { get; set; }
        public IEnumerable<int>? TagIds { get; set; }
    }
}
