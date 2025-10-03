using Application.Payment.DTOs;
using Core.Entities.Cart;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Payment.Services
{
    public interface IPaymentAppService
    {
        Task<ShoppingCart?> CreateOrUpdatePaymentIntent(string cartId);
        Task<string> RefundPayment(string paymentIntentId);
        Task<IReadOnlyList<DeliveryMethodDTO>> GetDeliveryMethodsAsync();
        
    }
}
