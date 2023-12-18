using System.ComponentModel.DataAnnotations;

namespace WebApplicationPustok.Models
{
    public class Author
    {
      
        public int Id { get; set; }
        [Required,MinLength(3),MaxLength(32)]
        public string Name { get; set; }
        [Required, MinLength(5), MaxLength(32)]
        public string Surname { get; set; }
       
    }
}
