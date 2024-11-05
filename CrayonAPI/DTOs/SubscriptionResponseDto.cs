namespace CrayonAPI.DTOs
{
    public class SubscriptionResponseDto
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public int ServiceCode { get; set; }
        public int Quantity { get; set; }
        public required string State { get; set; }
        public DateTime ValidTo { get; set; }
        public required string AccountName { get; set; }
    }
}
