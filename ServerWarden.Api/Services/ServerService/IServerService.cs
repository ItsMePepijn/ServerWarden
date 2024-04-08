using ServerWarden.Api.Models;

namespace ServerWarden.Api.Services.ServerService
{
	public interface IServerService
	{
		public Task<ServiceResult<List<ServerProfile>>> GetServerProfiles();
	}
}
