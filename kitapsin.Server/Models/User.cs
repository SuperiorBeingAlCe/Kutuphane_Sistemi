

using Microsoft.EntityFrameworkCore;

namespace kitapsin.Server.Models
{
    [Index(nameof(CardNumber), IsUnique = true)]
    public class User
    {
        public int Id { get; set; }

       
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;

       
        public string phoneNumber { get; set; } = null!;


        public string CardNumber { get; set; } = string.Empty;
        public ICollection<Loan> Loans { get; set; } = new List<Loan>();
        public ICollection<Penalty> Penalties { get; set; } = new List<Penalty>();
        public ICollection<Book> BorrowedBooks { get; set; } = new List<Book>();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? ExpireAt { get; set; }
}
}
