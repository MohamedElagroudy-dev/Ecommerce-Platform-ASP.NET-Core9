using API.Helper;
using Application.Common;
using Application.Orders.DTOs;
using Application.Orders.Services;
using Core.Entities.OrderAggregate;
using Core.Exceptions;
using Core.Sharing.Pagination;
using Core.Sharing.Pagination.Core.Sharing;
using Ecom.Application.Products.DTOs;
using Infrastructure.Repositories.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public AdminController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] OrderParams orderParams)
        {
            try
            {
                var orders = await _orderService.GetAllAsync(orderParams);
                return Ok(new ResponseAPI<PagedResult<OrderDto>>(200, "Orders fetched successfully", orders));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseAPI<string>(500, ex.Message));
            }
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseAPI<OrderDto>>> GetOrderById(int id)
        {
            try
            {
                var order = await _orderService.GetOrderByIdAsync(id);
                return Ok(new ResponseAPI<OrderDto>(200, data: order));
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new ResponseAPI(401));
            }
            catch (NotFoundException)
            {
                return NotFound(new ResponseAPI(404, $"Order with ID {id} not found"));
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
