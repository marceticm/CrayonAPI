using CrayonAPI.Enums;

namespace CrayonAPI.Entities
{
    public class Subscription
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public int ServiceCode { get; set; }
        public int Quantity { get; set; }
        public SubscriptionState State { get; set; }
        public DateTime ValidTo { get; set; }

        public required Account Account { get; set; }
    }
}
