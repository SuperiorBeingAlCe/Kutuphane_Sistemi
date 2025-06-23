namespace kitapsin.Server.Models
{
    public class Shelf
    {
        public int Id { get; set; }
        public string Section { get; set; } = string.Empty;
        public int Row { get; set; } 
        public int Column { get; set; } 
        public List<Book> Books { get; set; } = new();
    }
}
