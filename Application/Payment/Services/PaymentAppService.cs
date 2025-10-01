using Application.Payment.DTOs;
using Application.Payment.Mapping;
using Core.Entities;
using Core.Entities.Cart;
using Core.Entities.OrderAggregate;
using Core.Entities.Product;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Stripe;

namespace Application.Payment.Services
{
    public class PaymentAppService : IPaymentAppService
    {
        private readonly ICartService _cartService;
        private readonly IUnitOfWork _unit;
        private readonly IPaymentService _paymentGateway;
        private readonly string _whSecret;

        public PaymentAppService(ICartService cartService, IUnitOfWork unit, IPaymentService paymentGateway, IConfiguration config)
        {
            _cartService = cartService;
            _unit = unit;
            _paymentGateway = paymentGateway;
            _whSecret = config["Stripe:WhSecret"] ?? string.Empty;
        }

        public async Task<ShoppingCart?> CreateOrUpdatePaymentIntent(string cartId)
        {
            var cart = await _cartService.GetCartAsync(cartId)
                ?? throw new Exception("Cart unavailable");

            var shippingPrice = await GetShippingPriceAsync(cart) ?? 0;

            await ValidateCartItemsAsync(cart);

            var subtotal = CalculateSubtotal(cart);
            var total = subtotal + shippingPrice;

            var (paymentIntentId, clientSecret) = await _paymentGateway.CreateOrUpdatePaymentIntent(cart.PaymentIntentId, total);

            cart.PaymentIntentId = paymentIntentId;
            cart.ClientSecret = clientSecret;

            await _cartService.SetCartAsync(cart);

            return cart;
        }

        public async Task<string> RefundPayment(string paymentIntentId)
        {
            return await _paymentGateway.RefundPayment(paymentIntentId);
        }

        // Helpers
        private async Task ValidateCartItemsAsync(ShoppingCart cart)
        {
            foreach (var item in cart.Items)
            {
                var productItem = await _unit.Products.GetAsync(item.ProductId)
                    ?? throw new Exception("Problem getting product in cart");

                if (item.Price != productItem.Price)
                    item.Price = productItem.Price;
            }
        }

        private async Task<long?> GetShippingPriceAsync(ShoppingCart cart)
        {
            if (cart.DeliveryMethodId.HasValue)
            {
                var deliveryMethod = await _unit.DeliveryMethods
                    .GetAsync(cart.DeliveryMethodId.Value)
                    ?? throw new Exception("Problem with delivery method");

                return (long)deliveryMethod.Price * 100;
            }
            return null;
        }

        private long CalculateSubtotal(ShoppingCart cart)
        {
            return (long)cart.Items.Sum(x => x.Quantity * x.Price * 100);
        }

        public async Task<IReadOnlyList<DeliveryMethodDTO>> GetDeliveryMethodsAsync()
        {
            var methods = await _unit.DeliveryMethods.GetAllAsync();
            return methods.ToDtoList();
        }


        
    }
}

