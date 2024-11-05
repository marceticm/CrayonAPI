using CrayonAPI.Data;
using CrayonAPI.Entities;
using CrayonAPI.Enums;

namespace CrayonAPI.UnitTests.Data
{
    public class SubscriptionRepositoryTests : RepositoryTestBase
    {
        [Fact]
        public async Task AddSubscription_WhenCalled_AddsSubscriptionSuccessfully()
        {
            // Arrange
            using var context = await GetDbContext();
            var repository = new SubscriptionRepository(context);

            var customer = new Customer { Id = 1, Name = "Test Customer" };
            var account = new Account { Id = 1, AccountName = "Test Account", CustomerId = customer.Id, Customer = customer };
            var subscription = new Subscription
            {
                Id = 1,
                AccountId = account.Id,
                ServiceCode = 1,
                Quantity = 5,
                State = SubscriptionState.Active,
                ValidTo = DateTime.UtcNow.AddMonths(1),
                Account = account
            };

            context.Customers.Add(customer);
            context.Accounts.Add(account);
            await context.SaveChangesAsync();

            // Act
            var addedSubscription = await repository.AddSubscription(subscription);

            // Assert
            Assert.NotNull(addedSubscription);
            Assert.Equal(subscription.Id, addedSubscription.Id);
            Assert.Equal(subscription.AccountId, addedSubscription.AccountId);
            Assert.Equal(subscription.ServiceCode, addedSubscription.ServiceCode);
            Assert.Equal(subscription.Quantity, addedSubscription.Quantity);
        }

        [Fact]
        public async Task GetSubscriptionsByAccountId_WhenCalled_ReturnsSubscriptionsForAccount()
        {
            // Arrange
            using var context = await GetDbContext();
            var repository = new SubscriptionRepository(context);
            var customer = new Customer { Id = 1, Name = "Test Customer" };
            var account = new Account { Id = 1, AccountName = "Test Account", CustomerId = customer.Id, Customer = customer };

            context.Customers.Add(customer);
            context.Accounts.Add(account);

            context.Subscriptions.Add(new Subscription { Id = 1, AccountId = account.Id, ServiceCode = 1, Quantity = 3, Account = account });
            context.Subscriptions.Add(new Subscription { Id = 2, AccountId = account.Id, ServiceCode = 1, Quantity = 5, Account = account });
            await context.SaveChangesAsync();

            // Act
            var subscriptions = await repository.GetSubscriptionsByAccountId(account.Id);

            // Assert
            Assert.Equal(2, subscriptions.Count());
            Assert.All(subscriptions, s => Assert.Equal(account.Id, s.AccountId));
        }

        [Fact]
        public async Task UpdateSubscriptionQuantity_WhenCalled_UpdatesQuantitySuccessfully()
        {
            // Arrange
            using var context = await GetDbContext();
            var repository = new SubscriptionRepository(context);

            var customer = new Customer { Id = 1, Name = "Test Customer" };
            var account = new Account { Id = 1, AccountName = "Test Account", CustomerId = customer.Id, Customer = customer };

            context.Customers.Add(customer);
            context.Accounts.Add(account);

            var subscription = new Subscription { Id = 1, AccountId = account.Id, ServiceCode = 1, Quantity = 3, Account = account };
            context.Subscriptions.Add(subscription);
            await context.SaveChangesAsync();

            // Act
            var updatedSubscription = await repository.UpdateSubscriptionQuantity(subscription.Id, 10);

            // Assert
            Assert.NotNull(updatedSubscription);
            Assert.Equal(10, updatedSubscription.Quantity);
        }

        [Fact]
        public async Task GetSubscription_WhenCalled_ReturnsCorrectSubscription()
        {
            // Arrange
            using var context = await GetDbContext();
            var repository = new SubscriptionRepository(context);

            var customer = new Customer { Id = 1, Name = "Test Customer" };
            var account = new Account { Id = 1, AccountName = "Test Account", CustomerId = customer.Id, Customer = customer };
            var subscription = new Subscription { Id = 1, AccountId = account.Id, ServiceCode = 1, Account = account };

            context.Customers.Add(customer);
            context.Accounts.Add(account);
            context.Subscriptions.Add(subscription);
            await context.SaveChangesAsync();

            // Act
            var retrievedSubscription = await repository.GetSubscription(subscription.Id);

            // Assert
            Assert.NotNull(retrievedSubscription);
            Assert.Equal(subscription.Id, retrievedSubscription.Id);
            Assert.NotNull(retrievedSubscription.Account);
        }

        [Fact]
        public async Task CancelSubscription_WhenCalled_SetsStateToInactiveAndUpdatesValidToDate()
        {
            // Arrange
            using var context = await GetDbContext();
            var repository = new SubscriptionRepository(context);

            var customer = new Customer { Id = 1, Name = "Test Customer" };
            var account = new Account { Id = 1, AccountName = "Test Account", CustomerId = customer.Id, Customer = customer };
            var subscription = new Subscription { Id = 1, AccountId = account.Id, ServiceCode = 1, State = SubscriptionState.Active, ValidTo = DateTime.UtcNow.AddMonths(1), Account = account };

            context.Customers.Add(customer);
            context.Accounts.Add(account);
            context.Subscriptions.Add(subscription);
            await context.SaveChangesAsync();

            // Act
            var canceledSubscription = await repository.CancelSubscription(subscription.Id);

            // Assert
            Assert.NotNull(canceledSubscription);
            Assert.Equal(SubscriptionState.Inactive, canceledSubscription.State);
            Assert.True(canceledSubscription.ValidTo <= DateTime.UtcNow);
        }

        [Fact]
        public async Task ExtendSubscription_WhenCalled_UpdatesValidToDateSuccessfully()
        {
            // Arrange
            using var context = await GetDbContext();
            var repository = new SubscriptionRepository(context);
            var newValidToDate = DateTime.UtcNow.AddMonths(2);

            var customer = new Customer { Id = 1, Name = "Test Customer" };
            var account = new Account { Id = 1, AccountName = "Test Account", CustomerId = customer.Id, Customer = customer };
            var subscription = new Subscription { Id = 1, AccountId = account.Id, ServiceCode = 1, ValidTo = DateTime.UtcNow.AddMonths(1), Account = account };

            context.Customers.Add(customer);
            context.Accounts.Add(account);
            context.Subscriptions.Add(subscription);
            await context.SaveChangesAsync();

            // Act
            var extendedSubscription = await repository.ExtendSubscription(subscription.Id, newValidToDate);

            // Assert
            Assert.NotNull(extendedSubscription);
            Assert.Equal(newValidToDate, extendedSubscription.ValidTo);
        }
    }
}
