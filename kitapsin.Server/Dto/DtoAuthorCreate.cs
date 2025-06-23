using System.ComponentModel.DataAnnotations;

namespace kitapsin.Server.Dto
{
    public class DtoAuthorCreate
    {

        [MaxLength(30)]
        public string Name { get; set; } = null!;
    }
}
