using CrayonAPI.Enums;

namespace CrayonAPI.Entities
{
    public class Subscription
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public int ServiceId { get; set; }
        public required string Name { get; set; }
        public int Quantity { get; set; }
        public SoftwareState State { get; set; }
        public DateTime ValidTo { get; set; }

        public required Account Account { get; set; }
        public required Service Service { get; set; }
    }
}
