using System.ComponentModel.DataAnnotations;

namespace kitapsin.Server.Dto
{
    public class DtoPenaltyCreate
    {
        [Required]
        public int UserId { get; set; }
        [Required, MaxLength(300)]
        public string Reason { get; set; } = null!;
        [Required]
        public decimal Amount { get; set; }
    }
}
