using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using API.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace API.MiddleWare
{
    public class ExceptionMiddleware
    {
        private readonly IHostEnvironment _env;
        private readonly  RequestDelegate _next;  
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _env = env;
            _logger = logger;            
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;  //setting http status code for 500

                var responseMessage = _env.IsDevelopment() 
                                ? new ApiException((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace.ToString()) //while in DEVELOPMENT; return error messages 
                                : new ApiException((int)HttpStatusCode.InternalServerError);  //while in PRODUCTION; return error messages 

                var options = new JsonSerializerOptions{PropertyNamingPolicy = JsonNamingPolicy.CamelCase};
                var json = JsonSerializer.Serialize(responseMessage, options);

                await context.Response.WriteAsync(json);
            }
        }
    }
}