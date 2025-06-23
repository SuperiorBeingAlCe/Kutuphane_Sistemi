using System.ComponentModel.DataAnnotations;

namespace kitapsin.Server.Dto
{
    public class DtoUserCreate
    {
        [Required, MaxLength(100)]
        public string FullName { get; set; } = null!;

        [EmailAddress, MaxLength(100)]
        public string Email { get; set; } = null!;

        [Phone, MaxLength(20)]
        public string PhoneNumber { get; set; } = null!;

    
        public string? CardNumber { get; set; } = null!;

        public DateTime? ExpireAt { get; set; }
    }
}
