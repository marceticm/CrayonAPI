using CrayonAPI.Entities;

namespace CrayonAPI.Interfaces
{
    public interface ISubscriptionRepository
    {
        Task<Subscription> AddSubscription(Subscription subscription);
        Task<IEnumerable<Subscription>> GetSubscriptionsByAccountId(int accountId);
        Task<Subscription?> GetSubscription(int id);
        Task<Subscription?> UpdateSubscriptionQuantity(int subscriptionId, int quantity);
        Task<Subscription?> CancelSubscription(int subscriptionId);
        Task<Subscription?> ExtendSubscription(int subscriptionId, DateTime newValidToDate);
    }
}
