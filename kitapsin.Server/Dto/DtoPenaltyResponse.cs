namespace kitapsin.Server.Dto
{
    public class DtoPenaltyResponse
    {
        public int Id { get; set; }
        public string Reason { get; set; } = null!;
        public int UserId { get; set; }
        public decimal Amount { get; set; }
        public DateTime IssuedAt { get; set; }
        public bool IsPaid { get; set; }

       
    }
}
