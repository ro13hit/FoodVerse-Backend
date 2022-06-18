using FoodDeliverySampleApplication.API;
using FoodDeliverySampleApplication.DAL;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace FoodDeliverySampleApplication.Logic
{
    public class ItemRepository : IItemRepository
    {
        private readonly FoodDeliveryContext _context;
       
        public ItemRepository(FoodDeliveryContext context)
        {
            _context = context;
        }

        public async Task<ActionResult<IEnumerable<ItemResponse>>> GetItemsAsync()
        {
            var items = await _context.Item.Where(q => q.IsDeleted == 0 && q.IsAvailable == 1).Select(o => new ItemResponse
            {
                ItemId = o.ItemId,
                Name = o.Name,
                Description = o.Description,
                Price = o.Price,
                Quantity = o.Quantity,
                CategoryId = o.CategoryId,
                IsAvailable = o.IsAvailable,
                Image = o.Image
            }).OrderByDescending(q=> q.ItemId).ToListAsync();
            return items;
        }

        public async Task<ActionResult<ItemResponse>> GetItemById(int id)
        {
            var item = await _context.Item.FindAsync(id);
            if (item != null)
            {
                ItemResponse RequiredItem = new();
                RequiredItem.ItemId = item.ItemId;
                RequiredItem.Name = item.Name;
                RequiredItem.Description = item.Description;
                RequiredItem.Price = item.Price;
                RequiredItem.Image = item.Image;
                RequiredItem.Quantity = item.Quantity;
                RequiredItem.CategoryId = item.CategoryId;
                RequiredItem.IsAvailable = item.IsAvailable;
                return RequiredItem;
            }
            return null;
        }

        public async Task<ActionResult<Item>> AddItemAsync(ItemDTO request)
        {
            Item newItem = new();
            if (request.Name == null || request.Description == null || request.Price==0)
            {
                return null;
            }
            newItem.Name = request.Name;
            newItem.Description = request.Description;
            newItem.Price = request.Price;
            newItem.Image = request.Image;
            if(request.Quantity!=null)
            {
                newItem.Quantity = request.Quantity;
            }
            if(request.CategoryId!=0)
            {
                newItem.CategoryId = request.CategoryId;
            }
            newItem.IsAvailable = request.IsAvailable;
            _context.Item.Add(newItem);
            await _context.SaveChangesAsync();
            return newItem;
        }
        public async Task<ActionResult<ItemDTO>> UpdateItemAsync(int id, ItemDTO request)
        {
            var item = await _context.Item.FindAsync(id);
            if (item != null)
            {
                item.Name = request.Name;
                item.Description = request.Description;
                item.Price = request.Price;
                if (request.Quantity != null)
                {
                    item.Quantity = request.Quantity;
                }
                if (request.CategoryId >0)
                {
                    item.CategoryId = request.CategoryId;
                }
                if (request.Image != null)
                {
                    item.Image = request.Image;
                }
                await _context.SaveChangesAsync();
                return request;
            }
            return null;
        }

        public async Task<ActionResult<Item>> UpdateItemByPropertyAsync(int id, JsonPatchDocument patchItem)
        {
            var item = await _context.Item.FirstOrDefaultAsync(q => q.ItemId == id);
            if (item != null)
            {
                patchItem.ApplyTo(item);
                await _context.SaveChangesAsync();
                return item;
            }
            return null;
        }
        public async Task<ActionResult<Item>> DeleteItemAsync(int id)
        {
            var ItemToDelete = await _context.Item.FindAsync(id);
            if (ItemToDelete == null)
            {
                return null;
            }
            _context.Item.Remove(ItemToDelete);
            await _context.SaveChangesAsync();
            return ItemToDelete;
        }
    }
}
