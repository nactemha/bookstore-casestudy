using System.Security.Claims;
using ecommerce.extention;
using ecommerce.service;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class StatisticsController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly ICustomerService _customerService;
    private readonly ILogger<StatisticsController> _logger;

    public StatisticsController(IOrderService orderService, ICustomerService customerService, ILogger<StatisticsController> logger)
    {
        _orderService = orderService;
        _customerService = customerService;
        _logger = logger;
    }
    /// <summary>
    ///    Get all orders
    /// </summary>
    /// <returns></returns>
    /// <response code="200">Get all orders success</response>
    /// <response code="500">Internal Server Error</response>

    [HttpGet("monthly")]
    [FrontAuth]
    public async Task<IActionResult> GetMonthlyStatistics()
    {
        try
        {
            var customerEmail = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value;
            var customer = await _customerService.GetCustomerByEmailAsync(customerEmail);
            var monthlyStats = await _orderService.GetMonthlyStatisticsAsync(customer.Id);
            return Ok(monthlyStats);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting monthly statistics.");
            return StatusCode((int)System.Net.HttpStatusCode.InternalServerError);
        }

    }
}
