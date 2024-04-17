using ServerWarden.Api.Services.AuthService;

namespace ServerWarden.Api.Middleware
{
	public class UserIdProvider(RequestDelegate next)
	{
		private readonly RequestDelegate _next = next;

		public async Task InvokeAsync(HttpContext httpContext, IAuthService authService)
		{
			var authHeader = httpContext.Request.Headers.Authorization.ToString();

			if(string.IsNullOrWhiteSpace(authHeader))
			{
				await _next(httpContext);
				return;
			}

			var claims = authService.ParseClaimsFromJwt(authHeader);
			var userId = Guid.Parse(claims.First(claim => claim.Type.Equals("id")).Value);

			httpContext.Items.Add("UserId", userId);

			await _next(httpContext);
		}
	}

	public static class UserIdProviderExtensions
	{
		public static IApplicationBuilder UseUserIdProvider(this IApplicationBuilder builder)
		{
			return builder.UseMiddleware<UserIdProvider>();
		}
	}
}
