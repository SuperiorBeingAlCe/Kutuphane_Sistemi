using System.ComponentModel.DataAnnotations;

namespace kitapsin.Server.Dto
{
    public class DtoAdminLogin
    {
        [Required]
        public string Username { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;
    }
}
