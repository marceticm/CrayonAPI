using CrayonAPI.DTOs;
using CrayonAPI.Entities;

namespace CrayonAPI.Interfaces
{
    public interface ISubscriptionService
    {
        Task<Subscription> CreateSubscription(SubscriptionCreateDto subscriptionDto);
    }
}
