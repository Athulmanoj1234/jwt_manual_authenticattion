namespace jwtmanualauthentication.middleware
{
    public class AccessTokenMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        public async Task InvokeAsync(HttpContext context) {
            var isTokenExists = context.Request.Headers.TryGetValue("AccessToken", out var token);  //try get value performs on dictionary in key: value data and returns a booleon true or false
            if (isTokenExists && token.FirstOrDefault() == configuration["AccessToken"])
            {
                await next(context);
            }
            //this is eg for shortcircuiting middleware
            else {
                context.Response.StatusCode = 400;
            }
        }
    }
}
