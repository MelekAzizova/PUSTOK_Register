using WebApplicationPustok.Models;

namespace WebApplicationPustok.ViewModel.TagVM
{
    public class TagCreateVM
    {
       
        public string Title { get; set; }
        
        public ICollection<TagProduct>? TagProducts { get; set; }
    }
}
