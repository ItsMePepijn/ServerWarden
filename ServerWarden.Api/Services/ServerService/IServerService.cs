using ServerWarden.Api.Models;
using ServerWarden.Api.Models.Database;

namespace ServerWarden.Api.Services.ServerService
{
    public interface IServerService
	{
		public Task<ServiceResult<List<ServerProfile>>> GetUserServerProfiles(Guid userId);
		public Task<ServiceResult<ServerProfile>> CreateServer(Guid userId, ServerType serverType, string initialName);
	}
}
