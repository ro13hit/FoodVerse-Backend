using FoodDeliverySampleApplication.API;
using FoodDeliverySampleApplication.DAL;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodDeliverySampleApplication.Logic
{
    public interface IItemRepository
    {
        Task<ActionResult<IEnumerable<ItemResponse>>> GetItemsAsync();
        Task<ActionResult<Item>> AddItemAsync(ItemDTO request);
        Task<ActionResult<ItemDTO>> UpdateItemAsync(int id, ItemDTO request);
        Task<ActionResult<Item>> UpdateItemByPropertyAsync(int id, JsonPatchDocument patchItem);
        Task<ActionResult<Item>> DeleteItemAsync(int id);
        Task<ActionResult<ItemResponse>> GetItemById(int id);
    }
}
