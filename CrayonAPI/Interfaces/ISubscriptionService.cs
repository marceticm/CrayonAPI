using CrayonAPI.DTOs;

namespace CrayonAPI.Interfaces
{
    public interface ISubscriptionService
    {
        Task<SubscriptionResponseDto> CreateSubscription(int customerId, SubscriptionCreateDto subscriptionDto);
        Task<IEnumerable<SubscriptionResponseDto>> GetSubscriptionsByAccountId(int accountId, int customerId);
        Task<SubscriptionResponseDto> UpdateSubscriptionQuantity(int subscriptionId, int customerId, int quantity);
        Task<SubscriptionResponseDto> CancelSubscription(int subscriptionId, int customerId);
        Task<SubscriptionResponseDto> ExtendSubscription(int subscriptionId, int customerId, DateTime newValidToDate);
    }
}
