using Core.Entities;
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
        IGenericRepository<Category> Categories { get; }
        IGenericRepository<DeliveryMethod> DeliveryMethods { get; }
        IImageManagementService Images { get; }
        ICartService Cart { get; }
        Task<int> CompleteAsync();
    }
}
