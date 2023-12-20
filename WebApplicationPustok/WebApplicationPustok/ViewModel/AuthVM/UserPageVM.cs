using System.ComponentModel.DataAnnotations;

namespace WebApplicationPustok.ViewModel.AuthVM
{
    public class UserPageVM
    {
        [ MaxLength(36)]
        public string Fullname { get; set; }
        [ DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [ MaxLength(24)]
        public string Username { get; set; }
        public IFormFile ProfilePhoto { get; set; }
       
        public string NewPassword { get; set; }
    }

}

