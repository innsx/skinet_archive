using System;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IResponseCacheService
    {
         Task SetCacheResponseAsync(string cacheKey, object response, TimeSpan timeToLiver);
         Task<string> GetCachedResponseAsync(string cacheKey);
    }
}