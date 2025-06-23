

using System.ComponentModel.DataAnnotations;

namespace kitapsin.Server.Dto
{
    public class DtoShelf
    {
        [Required]
        public string Section { get; set; } = string.Empty;
        [Required]
        public int Row { get; set; }
        [Required]
        public int Column { get; set; }
       
    }
}
