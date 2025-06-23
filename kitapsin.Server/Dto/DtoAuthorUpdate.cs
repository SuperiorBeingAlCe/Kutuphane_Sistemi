using System.ComponentModel.DataAnnotations;

namespace kitapsin.Server.Dto
{
    public class DtoAuthorUpdate
    {
        [MaxLength(30)]
        public string Name { get; set; } = null!;
    }
}
