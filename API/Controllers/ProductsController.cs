using API.Helper;
using Core.Entities.Product;
using Core.Sharing;
using Ecom.Application.Products.DTOs;
using Ecom.Application.Products.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAll([FromQuery] ProductParams productParams)
        {
            var products = await _productService.GetAllAsync(productParams);
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var product = await _productService.GetProductAsync(id);
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromForm] AddProductDTO dto)
        {
            var product = await _productService.AddAsync(dto);
            return Ok(product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] UpdateProductDTO dto)
        {
            if (id != dto.Id)
                return BadRequest("Id mismatch");

            var product = await _productService.UpdateAsync(dto);
            return Ok(product);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productService.DeleteAsync(id);
            return Ok(product);
        }
    }
}
