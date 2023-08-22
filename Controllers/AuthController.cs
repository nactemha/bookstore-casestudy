using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ecommerce.extension;
using ecommerce.models;
using ecommerce.service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ecommerce.Controllers
{
    /// <summary>
    /// Auth Controller
    /// </summary>
    /// <remarks>
    /// This controller is used for login and register
    /// </remarks>
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> logger;
        private readonly ICustomerService customerService;
        private readonly ITokenService tokenService;

        public AuthController(ILogger<AuthController> logger, ICustomerService customerService, ITokenService tokenService)
        {
            this.logger = logger;
            this.customerService = customerService;
            this.tokenService = tokenService;
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <response code="200">Login success</response>
        /// <response code="401">Login fail</response>
        /// <response code="500">Internal Server Error</response>
        /// <remarks>
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            try
            {
                var isValid = await this.customerService.ValidateCustomerAsync(request.Email.Trim().ToLower(), request.Password.CalcMD5());
                if (!isValid)
                {
                    return Unauthorized();
                }
                var tokenInfo = tokenService.GetToken(request);
                if (tokenInfo.ErrorCode > 0)
                {
                    return Unauthorized();
                }
                return Ok(tokenInfo);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while login");
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }

        }
        /// <summary>
        /// Register
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <response code="200">Register success</response>
        /// <response code="400">Register fail</response>
        /// <response code="500">Internal Server Error</response>
        /// <remarks>
        /// Sample request:
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
           try
            {
                var customer = await this.customerService.GetCustomerByEmailAsync(request.Email);
                if (customer != null)
                {
                    return BadRequest("Email is already exist");

                }
                var newCustomer = new CustomerModel
                {
                    Name = request.Name,
                    Email = request.Email.Trim().ToLower(),
                    Password = request.Password,
                    Address = request.Address,
                    PhoneNumber = request.PhoneNumber,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                await this.customerService.AddCustomerAsync(newCustomer);
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while register");
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}


