using System.ComponentModel.DataAnnotations;

namespace kitapsin.Server.Dto
{
    public class DtoUserUpdate
    {
        [Required, MaxLength(100)]
        public string FullName { get; set; } = null!;

        [EmailAddress, MaxLength(100)]
        public string Email { get; set; } = null!;

        [Phone , MaxLength(20)]
        public string PhoneNumber { get; set; } = null!;

  
    }
}
