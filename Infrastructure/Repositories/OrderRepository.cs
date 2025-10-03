using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(
            string buyerEmail,
            params Expression<Func<Order, object>>[] includes)
        {
            IQueryable<Order> query = _context.Orders;

            foreach (var include in includes)
                query = query.Include(include);

            return await query
                .Where(o => o.BuyerEmail == buyerEmail)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }
    }
}
