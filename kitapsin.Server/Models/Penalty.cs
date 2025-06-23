

namespace kitapsin.Server.Models
{
    public class Penalty
    {
        public int Id { get; set; }

        
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public string Reason { get; set; } = null!;
        public decimal Amount { get; set; }
        public DateTime IssuedAt { get; set; } = DateTime.UtcNow;

        public bool IsPaid { get; set; } = false;
    }
}
