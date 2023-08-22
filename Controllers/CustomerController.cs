using ecommerce.extension;
using ecommerce.models;
using ecommerce.service;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace ecommerce.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ILogger<CustomersController> _logger;
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService, ILogger<CustomersController> logger)
        {
            _customerService = customerService;
            _logger = logger;
        }
        /// <summary>
        /// Get all customers
        /// </summary>
        /// <param name="customerRequest"></param>
        /// <returns></returns>
        /// <response code="200">Get all customers success</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet]
        [FrontAuth]
        public async Task<IActionResult> GetCurrent()
        {
            try
            {
                var claim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);
                if (claim == null)
                {
                    return Unauthorized();
                }
                var customerEmail = claim.Value;

                var customer = await _customerService.GetCustomerByEmailAsync(customerEmail);
                return Ok(customer);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error while getting current customer");
                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError);
            }
        }
        /// <summary>
        /// Get all customers
        /// </summary>
        /// <param name="customerRequest"></param>
        /// <returns></returns>
        /// <response code="200">Get all customers success</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPut()]
        [FrontAuth]
        public async Task<IActionResult> UpdateCustomer(CustomerModel customerRequest)
        {
            try
            {
                var customerEmail= User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value;
                var customer = await _customerService.GetCustomerByEmailAsync(customerEmail);
                customerRequest.Id = customer.Id;
                await _customerService.UpdateCustomerAsync(customerRequest);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Error while updating customer");
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get all customers
        /// </summary>
        /// <param name="customerRequest"></param>
        /// <returns></returns>
        /// <response code="200">Get all customers success</response>
        /// <response code="500">Internal Server Error</response>
        [HttpDelete()]
        [FrontAuth]
        public async Task<IActionResult> DeleteCustomer()
        {
            try
            {
                var customerEmail = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value;
                var customer = await _customerService.GetCustomerByEmailAsync(customerEmail);

                await _customerService.DeleteCustomerAsync(customer.Id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting customer");
                return NotFound(ex.Message);
            }
        }


    }

}


