using Application.Payment.DTOs;
using Application.Payment.Services;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentAppService _paymentService;
        private readonly IUnitOfWork _unit;

        public PaymentsController(IPaymentAppService paymentService, IUnitOfWork unit)
        {
            _paymentService = paymentService;
            _unit = unit;
        }

        [Authorize]
        [HttpPost("{cartId}")]
        public async Task<ActionResult> CreateOrUpdatePaymentIntent(string cartId)
        {
            var cart = await _paymentService.CreateOrUpdatePaymentIntent(cartId);
            if (cart == null) return BadRequest("Problem with your cart on the API");
            return Ok(cart);
        }

        [HttpGet("delivery-methods")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethodDTO>>> GetDeliveryMethods()
        {
            var methods = await _paymentService.GetDeliveryMethodsAsync();
            return Ok(methods);
        }

        // TODO: Wephook
    }

}
