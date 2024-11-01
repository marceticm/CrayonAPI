using CrayonAPI.Entities;

namespace CrayonAPI.Interfaces
{
    public interface ISubscriptionRepository
    {
        Task AddSubscription(Subscription subscription);
    }
}
