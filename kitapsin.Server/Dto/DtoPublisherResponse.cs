namespace kitapsin.Server.Dto
{
    public class DtoPublisherResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Address { get; set; } = string.Empty;


        public string Phone { get; set; } = string.Empty;


        public string Email { get; set; } = string.Empty;



    }
}
