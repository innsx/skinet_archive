using System.Collections.Generic;

namespace API.Errors
{
    public class ApiValidationErrorResponse : ApiResponse
    {
        private string[] strings;

        public ApiValidationErrorResponse() : base(400)  // using HTTP STATUS CODE 400
        {
            
        }

        public ApiValidationErrorResponse(string[] strings) : base(400)
        {
            this.strings = strings;
        }

        public IEnumerable<string> Errors { get; set; }
    }
}