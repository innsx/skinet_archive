namespace API.Errors
{
    public class ApiResponse
    {
        public ApiResponse(int statusCode, string message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }

        public int StatusCode { get; set; }
        public string Message { get; set; }

        private string GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "Client-side Error: You have made A BAD REQUEST!",
                401 => "Client-side Error: You are UNAUTHORIZED to make this REQUEST!",
                403 => "Client-side Error: You are FORBIDDEN to make this REQUEST!",
                404 => "Client-side Error: The Resources you've requested is NOT found!",
                500 => "Server-side Error: You've made an Internal Server Error!",
                501 => "Server-side Error: Not implemented!",
                502 => "Server-side Error: Bad Gateway Proxy Error!",
                503 => "Server-side Error: Services is Unavailable!",
                504 => "Server-side Error: Gateway timeout!",
                505 => "Server-side Error: HTTP Version Not Supported!",

                _ => null
            };
        }
    }
}