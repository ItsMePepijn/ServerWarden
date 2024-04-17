using ServerWarden.Api.Extensions;
using ServerWarden.Api.Models;
using ServerWarden.Api.Models.Dto;
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
            builder.MapGet("/{serverId}", GetServerProfileById);
            builder.MapPatch("/{serverId}/install", InstallServer);

			return builder;
        }

        private static async Task<IResult> GetServerProfiles(HttpContext context, IServerService serverService)
        {
			var userId = (Guid?)context.Items["UserId"] ?? Guid.Empty;

			var result = await serverService.GetUserServerProfiles(userId);
            return result.ToResponse();
        }

        private static async Task<IResult> CreateServer(CreateServerDto createServerDto, HttpContext context, IServerService serverService)
        {
			if (!Enum.IsDefined(typeof(ServerType), createServerDto.ServerType))
			{
				return new ServiceResult<ServerProfileDtoSimple>(ResultCode.InvalidServerType).ToResponse();
			}

			var userId = (Guid?)context.Items["UserId"] ?? Guid.Empty;

			var result = await serverService.CreateServer(userId, createServerDto.ServerType, createServerDto.Name);
			return result.ToResponse();
		}

        private static async Task<IResult> GetServerProfileById(Guid serverId, HttpContext context, IServerService serverService)
        {
			var userId = (Guid?)context.Items["UserId"] ?? Guid.Empty;

			var result = await serverService.GetServerProfileById(serverId, userId);
			return result.ToResponse();
		}

		private static async Task<IResult> InstallServer(Guid serverId, HttpContext context, IServerService serverService)
		{
			var userId = (Guid?)context.Items["UserId"] ?? Guid.Empty;

			var result = await serverService.InstallServer(serverId, userId);
			return result.ToResponse();
		}
    }
}
