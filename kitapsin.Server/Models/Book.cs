

namespace kitapsin.Server.Models
{
    public class Book
    {
        public int Id { get; set; }
   
        public string Title { get; set; } = null!;

      
        public int AuthorId { get; set; }
        public Author Author { get; set; } = null!;

 
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;


      

        public int PublicationYear { get; set; }

       
        public string ISBN { get; set; } = null!;

        public int Quantity { get; set; } = 1;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; } = true;

        public int PublisherId { get; set; }
        public Publisher Publisher { get; set; } = null!;
    }
}
