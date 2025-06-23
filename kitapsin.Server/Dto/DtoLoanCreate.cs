using System.ComponentModel.DataAnnotations;

namespace kitapsin.Server.Dto
{
    public class DtoLoanCreate
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public int BookId { get; set; }
        [Required]
        public string BookTitle { get; set; } = null!;
        [Required]
        public DateTime DueDate { get; set; }
    }
}
