namespace CrayonAPI.Entities
{
    public class Service
    {
        public int Id { get; set; }
        public required string ServiceName { get; set; }
        public required string Description { get; set; }
        public decimal Price { get; set; }
    }
}
