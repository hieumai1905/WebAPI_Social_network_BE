namespace Web_Social_network_BE.Middleware;

public class Authorization
{
    private readonly RequestDelegate _next;

    public Authorization(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/v1/api/admin"))
        {
            var role = context.Session.GetString("UserRole");
            if (string.IsNullOrEmpty(role) || role != "ADMIN_ROLE")
            {
                context.Response.StatusCode = 403;
                return;
            }
        }

        await _next(context);
    }
}