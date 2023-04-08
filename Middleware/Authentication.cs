namespace Web_Social_network_BE.Middleware;

public class Authentication
{
    private readonly RequestDelegate _next;

    public Authentication(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        context.Response.Headers.Add("Access-Control-Allow-Credentials", "true");
        if (!context.Request.Path.StartsWithSegments("/v1/api/login") &&
            !context.Request.Path.StartsWithSegments("/v1/api/register") &&
            !context.Request.Path.StartsWithSegments("/v1/api/register/confirm-code") &&
            !context.Request.Path.StartsWithSegments("/v1/api/forgot-password") &&
            !context.Request.Path.StartsWithSegments("/v1/api/codes/refresh"))
        {
            var userId = context.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                context.Response.StatusCode = 401;
                return;
            }

            var userStatus = context.Session.GetString("UserStatus");
            if (userStatus == "INACTIVE")
            {
                context.Response.StatusCode = 403;
                return;
            }
        }

        await _next.Invoke(context);
    }
}