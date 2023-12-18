using WebApplicationPustok.ViewModel.CategoryVM;
using WebApplicationPustok.ViewModel.ProductVM;
using WebApplicationPustok.ViewModel.SliderVM;

namespace WebApplicationPustok.ViewModel.HomeVM
{
    public class HomeVM
    {
        public IEnumerable<SliderListItemVM> Sliders { get; set; }
        public IEnumerable<ProductListItem> Products { get; set; }
        public IEnumerable<CategoryListItemVM> Categories { get; set; }
    }
}
