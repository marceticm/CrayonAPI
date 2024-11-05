namespace CrayonAPI.DTOs
{
    public class AccountCreateDto
    {
        public int CustomerId { get; set; }
        public required string AccountName { get; set; }
    }
}
