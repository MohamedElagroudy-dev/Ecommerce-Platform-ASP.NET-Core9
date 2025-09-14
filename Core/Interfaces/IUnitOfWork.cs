using Ecom.Core.Entities.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IProductRepository Products { get; }
        IGenericRepository<Photo> Photos { get; }
        public IGenericRepository<Category> Categories { get; }
        Task<int> CompleteAsync();
    }
}
