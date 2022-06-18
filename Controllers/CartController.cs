using FoodDeliverySampleApplication.API;
using FoodDeliverySampleApplication.DAL;
using FoodDeliverySampleApplication.Logic;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace FoodDeliverySampleApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository _cartRepository;
        private readonly ILogger<CartController> _logger;
        public CartController(ICartRepository cartRepository, ILogger<CartController> logger)
        {
            _cartRepository = cartRepository;
            _logger = logger;
        }

        [Authorize]
        [HttpGet("GetCart")]
        public ActionResult<List<Cart>> GetCart(int id)
        {
            _logger.LogInformation("Get cart informatation for user {id}", id);
            var NewCart = _cartRepository.GetCart(id);
            return Ok(NewCart);
        }

        [Authorize]
        [HttpPost("CreateCart")]
        public ActionResult<Cart> CreateCart(CartDTO request)
        {
            _logger.LogInformation("Creating cart for user {id}", request.UserId);
            var NewCart = _cartRepository.AddItemToCart(request);
            if (NewCart != null)
            {
                var message = "Item added to cart!";
                _logger.LogInformation(message);
                return Ok(
                    new
                    {
                        message,
                        request
                    }
                    );
            }
            _logger.LogWarning("Entered wrong item details to add to cart");
            return BadRequest("Provide correct details for adding items!");
        }

        [Authorize]
        [HttpPost("UpdateCart")]
        public ActionResult<Cart> UpdateCart(CartDTO request)
        {
            var cart = _cartRepository.UpdateCart(request);
            if(cart!=null)
            {
                return Ok(cart);
            }
            return NotFound("Item Not Found in cart!");
        }

        [Authorize]
        [HttpPost("PlaceOrder")]
        public async Task<ActionResult<Order>> PlaceOrder(OrderDTO request)
        {
            _logger.LogInformation("Placing order for cart items for {id}",request.UserId);
            var NewOrder = await _cartRepository.PlaceOrderAsync(request);
            if(NewOrder!=null)
            {
                var message = "Order Placed Successfully!";
                _logger.LogInformation(message);
                return Ok(
                    new
                    {
                        message,
                        NewOrder
                    }
                    );
            }
            _logger.LogWarning("Required details are missing to place the order");
            return BadRequest("Provide all necessary details to place order!");
        }

        [Authorize]
        [HttpPost("DeleteItemFromCart")]
        public ActionResult<Cart> DeleteItemFromCart(CartDTO request)
        {
            _logger.LogInformation("Deleting cart items for {id}", request.UserId);
            var Result = _cartRepository.DeleteItemFromCart(request);
            if(Result.Equals("Deleted"))
            {
                var message = "Item deleted!";
                _logger.LogInformation("Item deleted");
                return Ok(
                    new
                    {
                        message
                    }
                    );
            }
            _logger.LogWarning("Couldn't find the item required to delete");
            return NotFound();
        }

    }
}
