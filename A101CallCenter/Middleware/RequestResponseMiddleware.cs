namespace A101CallCenter.Middleware
{
    public class RequestResponseMiddleware
    {
        readonly RequestDelegate next;
        readonly ILogger<RequestResponseMiddleware> logger;

        public RequestResponseMiddleware(RequestDelegate Next, ILogger<RequestResponseMiddleware> Logger)
        {
            next = Next;
            logger = Logger;
        }

        public async Task Invoke(HttpContext context)
        {
            logger.LogInformation("Request: " + context.Request.Path);
            await next.Invoke(context);
            logger.LogInformation("Response: " + context.Response.StatusCode);
        }
    }
}
