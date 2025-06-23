using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace kitapsin.Server.Dto
{
    public class DtoBookUpdate
    {
        [Required]
        public string Title { get; set; } = null!;
        [Required]
        public int AuthorId { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public int PublisherId { get; set; }
       
        [Required]
        public int PublicationYear { get; set; }
        [RegularExpression(@"^(97(8|9))?\d{9}(\d|X)$", ErrorMessage = "Invalid ISBN format.")]
        public string ISBN { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public bool IsActive { get; set; }
    }
}
