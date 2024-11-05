using CrayonAPI.Entities;
using CrayonAPI.Enums;
using CrayonAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CrayonAPI.Data
{
    public class SubscriptionRepository : ISubscriptionRepository
    {
        private readonly CrayonDbContext _context;

        public SubscriptionRepository(CrayonDbContext context)
        {
            _context = context;
        }

        public async Task<Subscription> AddSubscription(Subscription subscription)
        {
            var result = _context.Subscriptions.Add(subscription);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<IEnumerable<Subscription>> GetSubscriptionsByAccountId(int accountId)
        {
            return await _context.Subscriptions
                .Where(s => s.AccountId == accountId)
                .ToListAsync();
        }

        public async Task<Subscription?> UpdateSubscriptionQuantity(int subscriptionId, int quantity)
        {
            var subscription = await _context.Subscriptions.FindAsync(subscriptionId);
            if (subscription != null)
            {
                subscription.Quantity = quantity;
                _context.Entry(subscription).Property(s => s.Quantity).IsModified = true;
                await _context.SaveChangesAsync();
            }
            return subscription;
        }

        public async Task<Subscription?> GetSubscription(int id)
        {
            return await _context.Subscriptions
                .Include(s => s.Account) 
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Subscription?> CancelSubscription(int subscriptionId)
        {
            var subscription = await _context.Subscriptions.FindAsync(subscriptionId);
            if (subscription != null)
            {
                subscription.State = SubscriptionState.Inactive;
                subscription.ValidTo = DateTime.UtcNow;
                _context.Entry(subscription).Property(s => s.State).IsModified = true;
                _context.Entry(subscription).Property(s => s.ValidTo).IsModified = true;
                await _context.SaveChangesAsync();
            }
            return subscription;
        }

        public async Task<Subscription?> ExtendSubscription(int subscriptionId, DateTime newValidToDate)
        {
            var subscription = await _context.Subscriptions.FindAsync(subscriptionId);
            if (subscription != null)
            {
                subscription.ValidTo = newValidToDate;
                _context.Entry(subscription).Property(s => s.ValidTo).IsModified = true;
                await _context.SaveChangesAsync();
            }
            return subscription;
        }
    }
}
