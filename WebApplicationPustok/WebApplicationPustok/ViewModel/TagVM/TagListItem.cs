using WebApplicationPustok.Models;

namespace WebApplicationPustok.ViewModel.TagVM
{
    public class TagListItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
       
        public ICollection<TagProduct>? TagProducts { get; set; }
    }
}
