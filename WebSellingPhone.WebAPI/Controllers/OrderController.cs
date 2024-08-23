using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        [HttpGet("get-all-order")]
        public async Task<IActionResult> GetQuizzes()
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
    }
}

   

