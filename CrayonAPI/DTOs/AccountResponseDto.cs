namespace CrayonAPI.DTOs
{
    public class AccountResponseDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public required string AccountName { get; set; }
        public required string CustomerName { get; set; }
    }
}
