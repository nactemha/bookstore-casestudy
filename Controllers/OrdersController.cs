using ecommerce.service;

namespace ecommerce.Controllers
{
    using ecommerce.extention;
    using ecommerce.models;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;

    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly ILogger<OrdersController> _logger;
        private readonly IOrderService _orderService;
        private readonly ICustomerService _customerService;

        public OrdersController(IOrderService orderService, ILogger<OrdersController> logger, ICustomerService customerService)
        {
            _orderService = orderService;
            _customerService = customerService;
            _logger = logger;
        }


        /// <summary>
        ///     Get all orders
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Get all orders success</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet]
        [FrontAuth]
        public async Task<IActionResult> GetCustomerOrders()
        {
            try
            {
                var customerEmail = User.Claims.FirstOrDefault(x => x.Type == System.Security.Claims.ClaimTypes.Email).Value;
                var customer = await _customerService.GetCustomerByEmailAsync(customerEmail);
                var orders = await _orderService.GetAllOrdersAsync();
                return Ok(orders);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error while getting all orders");
                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Get order by id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="200">Get order by id success</response>
        /// <response code="404">Order not found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet("{id:int}")]
        [FrontAuth]
        public async Task<IActionResult> GetOrderById(int id)
        {
            try
            {
                var order = await _orderService.GetOrderByIdAsync(id);
                if (order == null)
                {
                    return NotFound();
                }
                return Ok(order);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error while getting order by id");
                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Place order
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        /// <response code="201">Place order success</response>
        /// <response code="400">Place order fail</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost]
        [FrontAuth]
        public async Task<IActionResult> PlaceOrder(OrderEntity order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _orderService.PlaceOrderAsync(order);
                return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while placing order");
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Update order
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        /// <response code="200">Update order success</response>
        /// <response code="400">Update order fail</response>
        /// <response code="500">Internal Server Error</response>
        /// <remarks>
        [HttpDelete("{id:int}")]
        [FrontAuth]
        public async Task<IActionResult> CancelOrder(int id)
        {
            try
            {
                await _orderService.DeleteOrderAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting order");
                return NotFound(ex.Message);
            }
        }


    }



}


