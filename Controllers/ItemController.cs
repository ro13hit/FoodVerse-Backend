using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using FoodDeliverySampleApplication.Logic;
using FoodDeliverySampleApplication.DAL;
using FoodDeliverySampleApplication.API;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.JsonPatch;

namespace FoodDeliverySampleApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IItemRepository _itemRepository;
        private readonly ILogger<ItemController> _logger;

        public ItemController(IItemRepository itemRepository, ILogger<ItemController> logger)
        {
            _itemRepository = itemRepository;
            _logger = logger;
        }

        //item routes
        [HttpGet("GetItems")]
        public async Task<ActionResult<IEnumerable<Item>>> GetAllItems()
        {
            _logger.LogInformation("Getting all items!");
            var items = await _itemRepository.GetItemsAsync();
            return Ok(items);
        }

        [Authorize]
        [HttpPost("GetItem/{id}")]
        public async Task<ActionResult<ItemResponse>> GetItemById(int id)
        {
            var item = await _itemRepository.GetItemById(id);
            if(item!=null)
            {
                return Ok(item);
            }
            return NotFound("Item doesn't exists");
        }


        [Authorize]
        [HttpPost("AddItem")]
        public async Task<ActionResult<Item>> AddItem(ItemDTO request)
        {
            _logger.LogInformation("Adding new item with name {name}",request.Name);
            var item = await _itemRepository.AddItemAsync(request);
            if (item != null)
            {
                var message = "Item added successfully!";
                _logger.LogInformation(message);
                return Ok(new
                {
                    message,
                    request.Name,
                    request.Description,
                    request.Price
                });
            }
            _logger.LogWarning("Couldn't add item fields were not provided");
            return BadRequest("Enter all required details and try again!");
        }

        [Authorize]
        [HttpPut("UpdateItem/{id}")]
        public async Task<ActionResult<ItemDTO>> UpdateItem(int id, ItemDTO request)
        {
            _logger.LogInformation("Updating item {id}", id);
            var item = await _itemRepository.UpdateItemAsync(id,request);
            if (item != null)
            {
                var message = "Item updated successfully!";
                _logger.LogInformation(message);
                return Ok(new
                {
                    message,
                    request
                });
            }
            _logger.LogWarning("Item not found for id {id}", id);
            return NotFound("Check the id of item and try again");
        }

        [Authorize]
        [HttpPatch("UpdateItemByProperty/{id}")]
        public async Task<ActionResult<Item>> UpdateItemByProperty(int id, JsonPatchDocument request)
        {
            var updateItem = await _itemRepository.UpdateItemByPropertyAsync(id, request);
            if (updateItem != null)
            {
                var message = "Update Successfull";
                return Ok(new
                {
                    message,
                    updateItem
                });
            }
            return BadRequest("check categoryId and other details");
        }

        [Authorize]
        [HttpDelete("DeleteItem/{id}")]
        public async Task<ActionResult<Item>> DeleteItem(int id)
        {
            _logger.LogInformation("Deleting item with id {id}", id);
            var ItemToDelete = await _itemRepository.DeleteItemAsync(id);
            if (ItemToDelete != null)
            {
                var message = "Deleted Successfully";
                _logger.LogInformation(message);
                return Ok(
                    new
                    {
                        message,
                        ItemToDelete
                    });
            }
            _logger.LogWarning("Couldn't find item by id {id}", id);
            return NotFound("Check the id of item and try again!");
        }
    }
}
