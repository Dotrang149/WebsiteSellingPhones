using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebSellingPhone.Bussiness.Service;
using WebSellingPhone.Bussiness.ViewModel;
using WebSellingPhone.Data.Models;

namespace WebSellingPhone.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;

        }

        [Authorize(Policy = "AdminOnly")]
        [HttpGet("get-all-order")]
        public async Task<IActionResult> GetAllOrder()
        {
            var orders = await _orderService.GetAllAsync();

            var ordersViewModels = orders.Select(q => new OrderVm()
            {
                Id = q.Id,
                TotalAmount = q.TotalAmount,
                PaymentMethod = q.PaymentMethod,
                UserOrderId = q.UserOrderId,
            }).ToList();

            return Ok(ordersViewModels);
        }


        [HttpGet("get-orders-by-userId/{userId}")]
        public async Task<IActionResult> GetOrdersByUserId(Guid userId)
        {
            var orders = await _orderService.GetOrdersByUserIdAsync(userId);

            if (orders == null || !orders.Any())
            {
                return NotFound();
            }

            var ordersViewModels = orders.Select(q => new OrderVm()
            {
                Id = q.Id,
                TotalAmount = q.TotalAmount,
                PaymentMethod = q.PaymentMethod,
                UserOrderId = q.UserOrderId,
            }).ToList();

            return Ok(ordersViewModels);
        }

        [HttpGet("get-order/{id}")]
        public async Task<IActionResult> GetOrderById(Guid id)
        {
            var order = await _orderService.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            var orderVm = new OrderVm()
            {
                Id = order.Id,
                TotalAmount = order.TotalAmount,
                PaymentMethod = order.PaymentMethod,
                UserOrderId = order.UserOrderId,
            };

            return Ok(orderVm);
        }


        [Authorize(Policy = "CustomerOnly")]
        [HttpPost("create-order")]
        public async Task<IActionResult> CreateOrder([FromBody] OrderVm orderVm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var order = new Order()
            {
                TotalAmount = orderVm.TotalAmount,
                PaymentMethod = orderVm.PaymentMethod,
                UserOrderId = orderVm.UserOrderId,
            };

            await _orderService.AddAsync(order);
            return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, orderVm);
        }


        [Authorize(Policy = "CustomerOnly")]
        [HttpPut("update-order/{id}")]
        public async Task<IActionResult> UpdateOrder(Guid id, [FromBody] OrderVm orderVm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingOrder = await _orderService.GetByIdAsync(id);
            if (existingOrder == null)
            {
                return NotFound();
            }

            existingOrder.TotalAmount = orderVm.TotalAmount;
            existingOrder.PaymentMethod = orderVm.PaymentMethod;
            existingOrder.UserOrderId = orderVm.UserOrderId;

            await _orderService.UpdateAsync(existingOrder);
            return NoContent();
        }

        [Authorize(Policy = "CustomerOnly")]
        [HttpDelete("delete-order/{id}")]
        public async Task<IActionResult> DeleteOrder(Guid id)
        {
            var order = await _orderService.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            await _orderService.DeleteAsync(order);
            return NoContent();
        }

        [HttpGet("get-orders-by-userId/{userId}")]
        public async Task<IActionResult> GetOrdersByUserId(Guid userId)
        {
            var orders = await _orderService.GetOrdersByUserIdAsync(userId);

            if (orders == null || !orders.Any())
            {
                return NotFound();
            }

            var ordersViewModels = orders.Select(q => new OrderVm()
            {
                Id = q.Id,
                TotalAmount = q.TotalAmount,
                PaymentMethod = q.PaymentMethod,
                UserOrderId = q.UserOrderId,
            }).ToList();

            return Ok(ordersViewModels);
        }

    }
}
