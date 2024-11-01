namespace CrayonAPI.DTOs
{
    public class SubscriptionCreateDto
    {
        public int AccountId { get; set; }
        public int ServiceId { get; set; }
        public int Quantity { get; set; }
        public DateTime ValidTo { get; set; }
    }
}
