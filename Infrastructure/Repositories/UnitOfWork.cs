using Core.Entities.Product;
using Core.Interfaces;
using Ecom.Core.Entities.Product;
using Infrastructure.Persistence;

namespace Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public IProductRepository Products { get; }
        public IGenericRepository<Photo> Photos { get; }


        public UnitOfWork(ApplicationDbContext context,
                          IProductRepository productRepository,
                          IGenericRepository<Photo> photoRepository)
        {
            _context = context;
            Products = productRepository;
            Photos = photoRepository;
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
