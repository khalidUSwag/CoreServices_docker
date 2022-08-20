using Basket.Api.Entities;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Threading.Tasks;

namespace Basket.Api.Repository
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache distributedCache;
        public BasketRepository(IDistributedCache distributedCache_ )
        {
            distributedCache = distributedCache_?? throw new ArgumentNullException(nameof(distributedCache_));
        }
        public async Task DeleteBasket(string userName)
        {
           await distributedCache.RemoveAsync(userName);
        }

        public async Task<ShoppingCart> GetBasket(string userName)
        {
            var basket =await distributedCache.GetStringAsync(userName);
            if (string.IsNullOrEmpty(basket))
                return null;

            return Newtonsoft.Json.JsonConvert.DeserializeObject<ShoppingCart>(basket);
        }

        public async Task<ShoppingCart> UpdateBasket(ShoppingCart shoppingCart)
        {
            await distributedCache.SetStringAsync(shoppingCart.UserName, Newtonsoft.Json.JsonConvert.SerializeObject(shoppingCart));

            return await GetBasket(shoppingCart.UserName);
        }
    }
}
