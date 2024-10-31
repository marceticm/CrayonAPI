namespace CrayonAPI.Entities
{
    public class Account
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public required string AccountName { get; set; }

        public required Customer Customer { get; set; }
        public ICollection<Subscription> Subscriptions { get; set; } = [];
    }
}
