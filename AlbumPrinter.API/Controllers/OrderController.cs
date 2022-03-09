using AlbumPrinter.Common;
using AlbumPrinter.Dto;
using AlbumPrinter.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AlbumPrinter.API.Controllers
{
    /// <summary>
    /// Controller for management of Orders
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        /// <summary>
        /// Retrieves Order data based on the provided Order identifier
        /// </summary>
        /// <param name="orderId">Order identifier (unique)</param>
        /// <returns>Order details</returns>
        [HttpGet]
        public async Task<IActionResult> GetOrderDetails(Guid orderId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(CommonExceptionMessages.InvalidArgument);
            }
            try
            {
                var orderDetails = await _orderService.GetOrderInfo(orderId);

                return Ok(orderDetails);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Creates a new Order with provided products and their quantities.
        /// </summary>
        /// <param name="createOrderRequestDto">Order data containing products</param>
        /// <returns>Basic Order data such as OrderId and required BinWidth</returns>
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequestDto createOrderRequestDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(CommonExceptionMessages.InvalidArgument);
            }

            try
            {
                var orderBase = await _orderService.CreateOrder(createOrderRequestDto);

                return Ok(orderBase);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}