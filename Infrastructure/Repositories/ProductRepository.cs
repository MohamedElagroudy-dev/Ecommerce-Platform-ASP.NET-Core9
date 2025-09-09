using Core.Entities.Product;
using Core.Interfaces;
using Core.Sharing;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;

namespace Infrastructure.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllAsync(ProductParams productParams)
        {
            var query = _context.Products
                .Include(m => m.Category)
                .Include(m => m.Photos)
                .AsNoTracking();

            // Filtering by word
            if (!string.IsNullOrEmpty(productParams.Search))
            {
                var searchWords = productParams.Search.Split(' ');
                query = query.Where(m => searchWords.All(word =>
                    m.Name.ToLower().Contains(word.ToLower()) ||
                    m.Description.ToLower().Contains(word.ToLower())
                ));
            }

            // Filtering by category Id
            if (productParams.CategoryId.HasValue)
                query = query.Where(m => m.CategoryId == productParams.CategoryId);

            // Sorting
            if (!string.IsNullOrEmpty(productParams.Sort))
            {
                query = productParams.Sort switch
                {
                    "PriceAce" => query.OrderBy(m => m.Price),
                    "PriceDce" => query.OrderByDescending(m => m.Price),
                    _ => query.OrderBy(m => m.Name),
                };
            }

            productParams.TotatlCount = await query.CountAsync();

            // Pagination
            query = query
                .Skip(productParams.pageSize * (productParams.PageNumber - 1))
                .Take(productParams.pageSize);

            return await query.ToListAsync();
        }


        
    }
}
