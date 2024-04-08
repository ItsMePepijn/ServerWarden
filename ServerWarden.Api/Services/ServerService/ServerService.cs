using Microsoft.EntityFrameworkCore;
using ServerWarden.Api.Models;

namespace ServerWarden.Api.Services.ServerService
{
	public class ServerService(DataContext dataContext) : IServerService
	{
		private readonly DataContext _dataContext = dataContext;

		public async Task<ServiceResult<List<ServerProfile>>> GetServerProfiles()
		{
			try
			{
				var servers = await _dataContext.Servers.ToListAsync();

				return new(ResultCode.Success, servers);
			}
			catch (Exception)
			{
				return new(ResultCode.Failure);
			}
		}
	}
}
