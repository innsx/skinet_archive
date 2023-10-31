using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using StackExchange.Redis;

namespace Infrastructure.Data
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _redisDatabase;
        public BasketRepository(IConnectionMultiplexer redis)
        {
            _redisDatabase = redis.GetDatabase();            
        }

        public async Task<bool> DeleteBasketAsync(string basketId)
        {
            var isBasketIdDeleted = await _redisDatabase.KeyDeleteAsync(basketId);

            if (isBasketIdDeleted == false)
            {
                return false;
            }

            return true;
        }

        public async Task<CustomerBasket> GetBasketAsync(string basketId)
        {
            var data = await _redisDatabase.StringGetAsync(basketId);

            if (data.IsNullOrEmpty)
            {
                return null;
            }

            var deserializedData = JsonSerializer.Deserialize<CustomerBasket>(data);

            return deserializedData;
        }

        public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket)
        {
            var createdBasket = await _redisDatabase.StringSetAsync(basket.Id, JsonSerializer.Serialize(basket), TimeSpan.FromDays(30));

            if (!createdBasket)
            {
                return null;
            }

            var basketId = await GetBasketAsync(basket.Id);

            return basketId;
        }
    }
}