using System.ComponentModel.DataAnnotations;

namespace kitapsin.Server.Dto
{
    public class DtoBookCreate
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
        public string ISBN { get; set; } = null!;

        [Required]
        public int Quantity { get; set; }
    }
}
