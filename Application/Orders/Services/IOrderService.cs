using Application.Orders.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Orders.Services
{
    public interface IOrderService
    {
        Task<OrderDto> CreateOrderAsync(CreateOrderDto orderDto);
        Task<IReadOnlyList<OrderDto>> GetOrdersForUserAsync();
        Task<OrderDto> GetOrderByIdAsync(int orderId);
    }
}