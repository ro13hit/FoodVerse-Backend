using FoodDeliverySampleApplication.API;
using FoodDeliverySampleApplication.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodDeliverySampleApplication.Logic
{
    public class CartRepository : ICartRepository
    {
        private readonly FoodDeliveryContext _context;
        public CartRepository(FoodDeliveryContext context)
        {
            _context = context;
        }

        static Cart UserCart = new();
        
        //cart methods
        public List<ItemList> GetCart(int id)
        {
            return UserCart.Items;
        }
        public Cart AddItemToCart(CartDTO request)
        {
            if (request.ItemId < 1 || request.ItemCount < 1 || request.UserId<1)
            {
                return null;
            }
            if (UserCart.Items.Any(q => q.ItemId == request.ItemId))
            {
                var item = UserCart.Items.FirstOrDefault(q => q.ItemId == request.ItemId);
                item.ItemCount += request.ItemCount;
                return UserCart;
            }
            else
            {
                ItemList newItem = new();
                newItem.ItemId = request.ItemId;
                newItem.ItemCount = request.ItemCount;
                UserCart.Items.Add(newItem);
                UserCart.UserId = request.UserId;
                return UserCart;
            }
        }

        public Cart UpdateCart(CartDTO request)
        {
            if(request.ItemId<0 && request.UserId < 0)
            {
                return null;
            }
            var item = UserCart.Items.FirstOrDefault(q => q.ItemId == request.ItemId);
            if(item!=null)
            {
                item.ItemCount = request.ItemCount;
                return UserCart;
            }
            return null;
        }

        public async Task<Order> PlaceOrderAsync(OrderDTO request)
        { 
            if (request.UserId<1)
            {
                return null;
            }
            Order newOrder = new();
            newOrder.Address = request.Address;
            newOrder.UserId = request.UserId;
            if (request.Comments != null)
            {
                newOrder.Comments = request.Comments;
            }
            newOrder.TotalBill = request.TotalBill;
            _context.Order.Add(newOrder);
            await _context.SaveChangesAsync();
            int OrderId = await _context.Order.MaxAsync(q => q.OrderId);
            foreach (ItemList CurrentItem in UserCart.Items)
            {
                OrderToItem CurrentOrder = new();
                CurrentOrder.OrderId = OrderId;
                CurrentOrder.ItemId = CurrentItem.ItemId;
                CurrentOrder.ItemCount = CurrentItem.ItemCount;
                _context.OrderToItem.Add(CurrentOrder);
            }

            await _context.SaveChangesAsync();
            Order ResponseObject  = new();
            ResponseObject.Address = newOrder.Address;
            ResponseObject.UserId = newOrder.UserId;
            ResponseObject.TotalBill = newOrder.TotalBill;
            UserCart.Items.Clear();
            return ResponseObject;
        }

        public string DeleteItemFromCart(CartDTO request)
        {
            if(request.UserId<1 || request.ItemId<1)
            {
                return null;
            }
            UserCart.Items.RemoveAll(q=>q.ItemId==request.ItemId);
            return  "Deleted";
        }
    }
}
