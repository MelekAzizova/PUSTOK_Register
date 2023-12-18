using System.ComponentModel.DataAnnotations;

namespace WebApplicationPustok.ViewModel.SliderVM
{
    public class SliderUpdateVM
    {
        public string ImageUrl { get; set; }
        [Required, MinLength(5, ErrorMessage = "Wrong length"), MaxLength(64)]
        public string Title { get; set; }
        [Required, MinLength(5), MaxLength(64)]
        public string Description { get; set; }
        public byte Position { get; set; }
    }
}
