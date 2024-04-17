using ServerWarden.Api.Models;
using ServerWarden.Api.Models.Dto;

namespace ServerWarden.Api.Services.ServerService
{
    public interface IServerService
	{
		public Task<ServiceResult<List<ServerProfileDtoSimple>>> GetUserServerProfiles(Guid userId);
		public Task<ServiceResult<ServerProfileDtoSimple>> CreateServer(Guid userId, ServerType serverType, string initialName);
		public Task<ServiceResult<ServerProfileDto>> GetServerProfileById(Guid serverId, Guid userId);
		public Task<ServiceResult> InstallServer(Guid serverId, Guid userId);
		public Task<ServiceResult> StartServer(Guid serverId, Guid userId);
		public Task StartServer(Guid serverId);
		public Task ServerFinishedInstalling(Guid serverId);
	}
}
