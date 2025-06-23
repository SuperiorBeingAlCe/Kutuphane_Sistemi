using System.ComponentModel.DataAnnotations;

namespace kitapsin.Server.Dto
{
    public class DtoAdminCreate
    {
        [Required, MaxLength(30)]
        public string Username { get; set; } = null!;

        [Required, MaxLength(100)] 
        public string Password { get; set; } = null!;

        [Required, EmailAddress, MaxLength(100)]
        public string Email { get; set; } = null!;
    }
}
