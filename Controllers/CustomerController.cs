using ecommerce.extention;
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
        [HttpPut("{id:int}")]
        [FrontAuth]
        public async Task<IActionResult> UpdateCustomer(CustomerEntity customerRequest)
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
        [HttpDelete("{id:int}")]
        [FrontAuth]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            try
            {
                await _customerService.DeleteCustomerAsync(id);
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


