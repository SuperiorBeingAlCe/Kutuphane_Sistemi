using System.ComponentModel.DataAnnotations;

namespace kitapsin.Server.Dto
{
    public class DtoPublisherCreate
    {
        [Required, MaxLength(150)]
        public string Name { get; set; } = null!;

        [Required, MaxLength(150)]
        public string Address { get; set; } = string.Empty;

        [Required, Phone, MaxLength(20)]
        public string Phone { get; set; } = string.Empty;

        [Required, EmailAddress, MaxLength(100)]
        public string Email { get; set; } = string.Empty;
    }
}
