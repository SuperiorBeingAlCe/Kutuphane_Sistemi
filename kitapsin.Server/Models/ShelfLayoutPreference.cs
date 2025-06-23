namespace kitapsin.Server.Models
{
    public class ShelfLayoutPreference
    {
        public int Id { get; set; }

        public int AdminId { get; set; }

        /// <summary>
        /// true = A-Z Bloklar, false = A-Z Raflar
        /// </summary>
        public bool IsBlockLayout { get; set; }
    }
}
