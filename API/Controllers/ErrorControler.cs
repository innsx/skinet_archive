using API.Errors;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("errors/{code}")]
     [ApiExplorerSettings(IgnoreApi = true)]   // ignored by API.Controller to not LOOKING FOR ANY MATCHING ROUTE
    public class ErrorController : BaseApiController
    {
        public IActionResult Error(int code)
        {
            return new ObjectResult(new ApiResponse(code));
        }
    }
}