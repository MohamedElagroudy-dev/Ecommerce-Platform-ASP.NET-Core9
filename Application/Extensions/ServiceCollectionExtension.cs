


using Application.Categories.Services;
using Core.Interfaces;
using Ecom.Application.Products.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;


namespace Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplication(this IServiceCollection services)
        {
            var applicationAssembly = typeof(ServiceCollectionExtensions).Assembly;

            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();

        }


    }
}