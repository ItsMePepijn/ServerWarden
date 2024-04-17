using Microsoft.AspNetCore.SignalR;
using ServerWarden.Api.Extensions;
using ServerWarden.Api.Models;
using ServerWarden.Api.Services.ServerService;

namespace ServerWarden.Api.Hubs
{
    public class ServerHub(IServerService serverService) : Hub
    {
        private readonly IServerService _serverService = serverService;

        public async Task<IResult> JoinServerGroup(Guid serverId)
        {
			var httpContext = Context.GetHttpContext();
			if (httpContext is null)
				return new ServiceResult(ResultCode.Failure).ToResponse();

			var userIdClaim = httpContext.User.Claims.FirstOrDefault(c => c.Type.Equals("id"));
			if (userIdClaim is null)
				return new ServiceResult(ResultCode.Failure).ToResponse();

			var userId = Guid.Parse(userIdClaim.Value);

			var result = await _serverService.GetServerProfileById(serverId, userId);

			if (result.Code != ResultCode.Success)
				return result.ToResponse();

			await Groups.AddToGroupAsync(Context.ConnectionId, serverId.ToString());

			return result.ToResponse();
		}
    }

    public class ServerHubService(IHubContext<ServerHub> serverHub)
    {
        private readonly IHubContext<ServerHub> _serverHub = serverHub;

		public void ServerUpdated(Guid serverId)
		{
			_serverHub.Clients.Group(serverId.ToString()).SendAsync("ServerUpdate");
		}

		public void ServerInstallLog(Guid serverId, string log)
		{
			_serverHub.Clients.Group(serverId.ToString()).SendAsync("ServerInstallLog", log);
		}
	}
}
