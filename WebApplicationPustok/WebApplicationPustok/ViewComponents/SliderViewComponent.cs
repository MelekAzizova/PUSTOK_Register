
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationPustok.Context;
using WebApplicationPustok.ViewModel.SliderVM;

namespace WebApplicationPustok.ViewComponents
{
    public class SliderViewComponent: ViewComponent
    {
       PustokDbContext _context { get; }

        public SliderViewComponent(PustokDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await _context.Sliders.Select(s => new SliderListItemVM
            {
                Id = s.Id,
                ImageUrl = s.ImageUrl,
                IsLeft = s.IsLeft,
                Title = s.Title,
                
            }).ToListAsync());
        }
    }
}
