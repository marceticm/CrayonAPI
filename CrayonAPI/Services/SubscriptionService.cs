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

        public async Task<SubscriptionResponseDto> CreateSubscription(int customerId, SubscriptionCreateDto subscriptionDto)
        {
            if (subscriptionDto.ValidTo <= DateTime.UtcNow)
            {
                throw new ArgumentException("The expiration date must be in the future.");
            }

            var account = await _accountRepository.GetAccount(subscriptionDto.AccountId);
            if (account == null || account.CustomerId != customerId)
            {
                throw new ArgumentException("Invalid AccountId or CustomerId");
            }

            var service = await _ccpService.GetCCPService(subscriptionDto.ServiceCode);
            if (service == null)
            {
                throw new ArgumentException("Invalid ServiceId");
            }

            var subscription = new Subscription
            {
                AccountId = subscriptionDto.AccountId,
                ServiceCode = subscriptionDto.ServiceCode,
                Quantity = subscriptionDto.Quantity,
                State = SubscriptionState.Active,
                ValidTo = subscriptionDto.ValidTo,
                Account = account
            };

            var createdSubscription = await _subscriptionRepository.AddSubscription(subscription);

            return new SubscriptionResponseDto
            {
                Id = createdSubscription.Id,
                AccountId = createdSubscription.AccountId,
                ServiceCode = createdSubscription.ServiceCode,
                Quantity = createdSubscription.Quantity,
                State = createdSubscription.State.ToString(),
                ValidTo = createdSubscription.ValidTo,
                AccountName = createdSubscription.Account.AccountName
            };
        }

        // We are including the customerId here to prevent a customer from seeing another customer's accounts
        public async Task<IEnumerable<SubscriptionResponseDto>> GetSubscriptionsByAccountId(int accountId, int customerId)
        {
            var account = await _accountRepository.GetAccount(accountId);
            if (account == null || account.CustomerId != customerId)
            {
                throw new ArgumentException("Invalid AccountId or CustomerId");
            }

            var subscriptions = await _subscriptionRepository.GetSubscriptionsByAccountId(accountId);

            return subscriptions.Select(s => new SubscriptionResponseDto
            {
                Id = s.Id,
                AccountId = s.AccountId,
                ServiceCode = s.ServiceCode,
                Quantity = s.Quantity,
                State = s.State.ToString(),
                ValidTo = s.ValidTo,
                AccountName = account.AccountName
            }).ToList();
        }

        public async Task<SubscriptionResponseDto> UpdateSubscriptionQuantity(int subscriptionId, int customerId, int quantity)
        {
            if (quantity <= 0)
            {
                throw new ArgumentException("Quantity must be greater than zero.");
            }

            var subscription = await _subscriptionRepository.GetSubscription(subscriptionId);
            if (subscription == null || subscription.Account.CustomerId != customerId)
            {
                throw new ArgumentException("Invalid SubscriptionId or CustomerId.");
            }

            var updatedSubscription = await _subscriptionRepository.UpdateSubscriptionQuantity(subscriptionId, quantity);
            if (updatedSubscription == null)
            {
                throw new InvalidOperationException("Failed to update subscription quantity.");
            }

            return new SubscriptionResponseDto
            {
                Id = updatedSubscription.Id,
                AccountId = updatedSubscription.AccountId,
                ServiceCode = updatedSubscription.ServiceCode,
                Quantity = updatedSubscription.Quantity,
                State = updatedSubscription.State.ToString(),
                ValidTo = updatedSubscription.ValidTo,
                AccountName = updatedSubscription.Account.AccountName
            };
        }

        public async Task<SubscriptionResponseDto> CancelSubscription(int subscriptionId, int customerId)
        {
            var subscription = await _subscriptionRepository.GetSubscription(subscriptionId);
            if (subscription == null || subscription.Account.CustomerId != customerId)
            {
                throw new ArgumentException("Invalid SubscriptionId or CustomerId");
            }

            var canceledSubscription = await _subscriptionRepository.CancelSubscription(subscriptionId);
            if (canceledSubscription == null)
            {
                throw new InvalidOperationException("Failed to cancel subscription.");
            }

            return new SubscriptionResponseDto
            {
                Id = canceledSubscription.Id,
                AccountId = canceledSubscription.AccountId,
                ServiceCode = canceledSubscription.ServiceCode,
                Quantity = canceledSubscription.Quantity,
                State = canceledSubscription.State.ToString(),
                ValidTo = canceledSubscription.ValidTo,
                AccountName = canceledSubscription.Account.AccountName
            };
        }

        public async Task<SubscriptionResponseDto> ExtendSubscription(int subscriptionId, int customerId, DateTime newValidToDate)
        {
            if (newValidToDate <= DateTime.UtcNow)
            {
                throw new ArgumentException("The new expiration date must be in the future.");
            }

            var subscription = await _subscriptionRepository.GetSubscription(subscriptionId);
            if (subscription == null || subscription.Account.CustomerId != customerId)
            {
                throw new ArgumentException("Invalid SubscriptionId or CustomerId.");
            }

            if (subscription.State != SubscriptionState.Active)
            {
                throw new InvalidOperationException("Only active subscriptions can be extended.");
            }

            var extendedSubscription = await _subscriptionRepository.ExtendSubscription(subscriptionId, newValidToDate);
            if (extendedSubscription == null)
            {
                throw new InvalidOperationException("Failed to extend subscription.");
            }

            return new SubscriptionResponseDto
            {
                Id = extendedSubscription.Id,
                AccountId = extendedSubscription.AccountId,
                ServiceCode = extendedSubscription.ServiceCode,
                Quantity = extendedSubscription.Quantity,
                State = extendedSubscription.State.ToString(),
                ValidTo = extendedSubscription.ValidTo,
                AccountName = extendedSubscription.Account.AccountName
            };
        }
    }
}
