using CrayonAPI.Entities;
using CrayonAPI.Interfaces;

namespace CrayonAPI.Data
{
    public class SubscriptionRepository : ISubscriptionRepository
    {
        private readonly CrayonDbContext _context;

        public SubscriptionRepository(CrayonDbContext context)
        {
            _context = context;
        }

        public async Task AddSubscription(Subscription subscription)
        {
            _context.Subscriptions.Add(subscription);
            await _context.SaveChangesAsync();
        }
    }
}
