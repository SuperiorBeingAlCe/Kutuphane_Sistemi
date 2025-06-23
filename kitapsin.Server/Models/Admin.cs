

namespace kitapsin.Server.Models
{
    public class Admin
    {
        public int Id { get; set; }

       
        public string Username { get; set; } = null!;

       
        public string PasswordHash { get; set; } = null!;

       
        public string Email { get; set; } = null!;


    }
}
