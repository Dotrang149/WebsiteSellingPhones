using Microsoft.AspNetCore.Mvc;
using WebSellingPhone.Bussiness.Service;
using WebSellingPhone.Bussiness.ViewModel;
using WebSellingPhone.Bussiness.ViewModel.Mappers;
using WebSellingPhone.Data.Models;

namespace WebSellingPhone.WebAPI.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("get-all-orders")]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _orderService.GetAllAsync();
            var ordersViewModels = orders.Select(o => o.ToOrderVm());
            return Ok(ordersViewModels);
        }

        [HttpGet("get-by-id/{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var order = await _orderService.GetByIdAsync(id);

            if (order == null)
            {
                return NotFound();
            }
            return Ok(order.ToOrderVm());
        }

        [HttpPost("create-order")]
        public async Task<IActionResult> Create([FromBody] OrderVm orderVm)
        {
            if (orderVm == null)
            {
                return BadRequest("Order data is null");
            }

            var order = orderVm.ToOrder();

            var result = await _orderService.AddAsync(order);

            if (result > 0)
            {
                return CreatedAtAction(nameof(GetById), new { id = order.Id }, order.ToOrderVm());
            }
            return BadRequest("Failed to create order");
        }

        [HttpPut("update-order/{id}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] OrderVm orderVm)
        {
            if (orderVm == null || id == Guid.Empty)
            {
                return BadRequest("Invalid order data");
            }

            var existingOrder = await _orderService.GetByIdAsync(id);

            if (existingOrder == null)
            {
                return NotFound();
            }

            var updatedOrder = orderVm.ToOrder();
            updatedOrder.Id = id;

            var result = await _orderService.UpdateAsync(updatedOrder);

            if (result > 0)
            {
                return NoContent();
            }
            return BadRequest("Failed to update order");
        }

        [HttpDelete("delete-order/{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Invalid order Id");
            }

            var result = await _orderService.DeleteAsync(id);

            if (result)
            {
                return NoContent();
            }
            return NotFound();
        }
    }
}
