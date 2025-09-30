using Core.Entities.OrderAggregate;
using System.Linq.Expressions;

namespace Core.Interfaces
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<IReadOnlyList<Order>> GetOrdersForUserAsync(
            string buyerEmail,
            params Expression<Func<Order, object>>[] includes);
    }
}