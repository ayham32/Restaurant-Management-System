namespace RestaurantApi
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                   await _next(context);
            }
            catch(Exception e)
            {
                   
                await HandleExceptionAsync(context,e);
            }
        }

        private Task HandleExceptionAsync(HttpContext context,Exception e)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 500;

            var result = new
            {
                message = "Something went wrong. Please try again." 
            };

            return context.Response.WriteAsJsonAsync(result);
        }
    }
}
