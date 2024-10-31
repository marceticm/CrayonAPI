namespace CrayonAPI.Entities
{
    public class Customer
    {
        public int Id { get; set; }
        public required string Name { get; set; }

        public ICollection<Account> Accounts { get; set; } = [];
    }
}
