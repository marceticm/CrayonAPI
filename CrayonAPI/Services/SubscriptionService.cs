using CrayonAPI.DTOs;
using CrayonAPI.Entities;
using CrayonAPI.Enums;
using CrayonAPI.Interfaces;

namespace CrayonAPI.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly ICCPService _ccpService;

        public SubscriptionService(
            ISubscriptionRepository subscriptionRepository,
            IAccountRepository accountRepository,
            ICCPService ccpService)
        {
            _subscriptionRepository = subscriptionRepository;
            _accountRepository = accountRepository;
            _ccpService = ccpService;
        }

        public async Task<Subscription> CreateSubscription(SubscriptionCreateDto subscriptionDto)
        {
            var account = await _accountRepository.GetAccount(subscriptionDto.AccountId);
            if (account == null)
            {
                throw new ArgumentException("Invalid AccountId");
            }

            var service = await _ccpService.GetCCPService(subscriptionDto.ServiceId);
            if (service == null)
            {
                throw new ArgumentException("Invalid ServiceId");
            }

            var subscription = new Subscription
            {
                AccountId = subscriptionDto.AccountId,
                ServiceId = subscriptionDto.ServiceId,
                Quantity = subscriptionDto.Quantity,
                State = SubscriptionState.Active,
                ValidTo = subscriptionDto.ValidTo,
                Account = account,
                Service = service
            };

            await _subscriptionRepository.AddSubscription(subscription);
            return subscription;
        }
    }
}
