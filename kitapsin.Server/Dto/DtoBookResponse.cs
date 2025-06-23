namespace kitapsin.Server.Dto
{
    public class DtoBookResponse
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public int AuthorId { get; set; } 
        public int CategoryId { get; set; } 
   
        public int PublicationYear { get; set; }
        public string ISBN { get; set; } = null!;
        public int Quantity { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }

        public int PublisherId { get; set; }

    }
}
