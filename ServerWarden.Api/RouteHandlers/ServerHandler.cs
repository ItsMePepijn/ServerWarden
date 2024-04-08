using ServerWarden.Api.Extensions;
using ServerWarden.Api.Models;
using ServerWarden.Api.Services.AuthService;
using ServerWarden.Api.Services.ServerService;

namespace ServerWarden.Api.RouteHandlers
{
    public static class ServerHandler
    {
        public static RouteGroupBuilder MapServerRoutes(this RouteGroupBuilder builder)
        {
            builder.MapGet("/", GetServerProfiles);
            builder.MapPost("/", CreateServer);

            return builder;
        }

        private static async Task<IResult> GetServerProfiles(HttpContext context, IAuthService authService, IServerService serverService)
        {
			var claims = authService.ParseClaimsFromJwt(context.Request.Headers.Authorization!);
			var userId = Guid.Parse(claims.First(claim => claim.Type.Equals("id")).Value);

			var result = await serverService.GetServerProfiles();
            return result.ToResponse();
        }

        private static async Task<IResult> CreateServer(ServerType type, string serverName, HttpContext context, IAuthService authService, IServerService serverService)
        {
			var claims = authService.ParseClaimsFromJwt(context.Request.Headers.Authorization!);
			var userId = Guid.Parse(claims.First(claim => claim.Type.Equals("id")).Value);

			var result = await serverService.CreateServer(userId, type, serverName);
			return result.ToResponse();
		}
    }
}
