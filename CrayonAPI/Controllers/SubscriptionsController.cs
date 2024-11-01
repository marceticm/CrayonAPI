using CrayonAPI.DTOs;
using CrayonAPI.Entities;
using CrayonAPI.Interfaces;
using Microsoft.AspNetCore.Http;
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

        [HttpPost]
        public async Task<ActionResult<Subscription>> PurchaseSubscription([FromBody] SubscriptionCreateDto subscriptionDto)
        {
            if (subscriptionDto == null)
            {
                return BadRequest("Subscription data is invalid.");
            }

            try
            {
                var newSubscription = await _subscriptionService.CreateSubscription(subscriptionDto);
                return CreatedAtAction(nameof(PurchaseSubscription), new { id = newSubscription.Id }, newSubscription);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
