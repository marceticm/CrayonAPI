using CrayonAPI.DTOs;
using CrayonAPI.Entities;
using CrayonAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CrayonAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionsController : ControllerBase
    {
        private readonly ISubscriptionService _subscriptionService;

        public SubscriptionsController(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        [HttpPost("customer/{customerId}")]
        public async Task<ActionResult<Subscription>> PurchaseSubscription(int customerId, [FromBody] SubscriptionCreateDto subscriptionDto)
        {
            if (subscriptionDto == null)
            {
                return BadRequest("Subscription data is invalid.");
            }

            try
            {
                var newSubscription = await _subscriptionService.CreateSubscription(customerId, subscriptionDto);
                return CreatedAtAction(nameof(PurchaseSubscription), new { id = newSubscription.Id }, newSubscription);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("customer/{customerId}/account/{accountId}")]
        public async Task<ActionResult<IEnumerable<SubscriptionResponseDto>>> GetSubscriptionsByAccountId(int customerId, int accountId)
        {
            try
            {
                var subscriptions = await _subscriptionService.GetSubscriptionsByAccountId(accountId, customerId);
                return Ok(subscriptions);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("customer/{customerId}/subscription/{subscriptionId}/quantity")]
        public async Task<ActionResult<SubscriptionResponseDto>> UpdateSubscriptionQuantity(int customerId, int subscriptionId, [FromBody] int quantity)
        {
            try
            {
                var updatedSubscription = await _subscriptionService.UpdateSubscriptionQuantity(subscriptionId, customerId, quantity);
                return Ok(updatedSubscription);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPatch("customer/{customerId}/subscription/{subscriptionId}/cancel")]
        public async Task<ActionResult<SubscriptionResponseDto>> CancelSubscription(int customerId, int subscriptionId)
        {
            try
            {
                var canceledSubscription = await _subscriptionService.CancelSubscription(subscriptionId, customerId);
                return Ok(canceledSubscription);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPatch("customer/{customerId}/subscription/{subscriptionId}/extend")]
        public async Task<ActionResult<SubscriptionResponseDto>> ExtendSubscription(int customerId, int subscriptionId, [FromBody] DateTime newValidToDate)
        {
            try
            {
                var extendedSubscription = await _subscriptionService.ExtendSubscription(subscriptionId, customerId, newValidToDate);
                return Ok(extendedSubscription);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
