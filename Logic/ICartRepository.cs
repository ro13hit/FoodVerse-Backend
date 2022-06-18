using FoodDeliverySampleApplication.API;
using FoodDeliverySampleApplication.DAL;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodDeliverySampleApplication.Logic
{
    public interface ICartRepository
    {
        Cart AddItemToCart(CartDTO request);
        Cart UpdateCart(CartDTO request);
        Task<Order> PlaceOrderAsync(OrderDTO request);
        List<ItemList> GetCart(int id);
        string DeleteItemFromCart(CartDTO request);
    }
}
