using ServerWarden.Api.Models;
using ServerWarden.Api.Models.Database;
using ServerWarden.Api.Models.Dto;

namespace ServerWarden.Api.Services.ServerService
{
    public interface IServerService
	{
		public Task<ServiceResult<List<ServerProfileDtoSimple>>> GetUserServerProfiles(Guid userId);
		public Task<ServiceResult<ServerProfile>> CreateServer(Guid userId, ServerType serverType, string initialName);
		public Task<ServiceResult<ServerProfileDto>> GetServerProfileById(Guid serverId, Guid userId);
	}
}
