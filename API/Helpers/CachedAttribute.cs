using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace API.Helpers
{
    public class CachedAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _timeToLiveInSeconds;
        public CachedAttribute(int timeToLiveInSeconds)
        {
            _timeToLiveInSeconds = timeToLiveInSeconds;
        }


        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cachedService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();

            var cacheKey = GeneratedCachedKeyFromRequest(context.HttpContext.Request);

            var cachedResponse = await cachedService.GetCachedResponseAsync(cacheKey);

            if (!string.IsNullOrEmpty(cachedResponse))
            {
                var contentResult = new ContentResult
                {
                    Content = cachedResponse,
                    ContentType = "application/json",
                    StatusCode = 200
                };

                context.Result = contentResult;

                return;
            }

            var executedContext = await next(); // move to controller class

            if (executedContext.Result is OkObjectResult okObjectResult)
            {
                await cachedService.SetCacheResponseAsync(cacheKey, okObjectResult.Value, TimeSpan.FromSeconds(_timeToLiveInSeconds));
            }
        }

        private string GeneratedCachedKeyFromRequest(HttpRequest httpRequest)
        {
            var keyBuilder = new StringBuilder();

            keyBuilder.Append($"{httpRequest.Path}");

            foreach (var (key, value) in httpRequest.Query.OrderBy(x => x.Key))
            {
                keyBuilder.Append($"{key}-{value}");
            }

            return keyBuilder.ToString();
        }

    }
}