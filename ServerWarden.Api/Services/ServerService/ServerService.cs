using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ServerWarden.Api.Models;
using ServerWarden.Api.Models.Database;
using ServerWarden.Api.Models.Dto;
using ServerWarden.Api.Services.SteamService;
using ServerWarden.Api.Settings;

namespace ServerWarden.Api.Services.ServerService
{
    public class ServerService(DataContext dataContext, ISteamService steamService, IOptions<Paths> paths) : IServerService
	{
		private readonly DataContext _dataContext = dataContext;
		private readonly ISteamService _steamService = steamService;
		private readonly Paths _paths = paths.Value;

		public async Task<ServiceResult<List<ServerProfileDtoSimple>>> GetUserServerProfiles(Guid userId)
		{
			try
			{
				var servers = await _dataContext.Servers
					.Include(x => x.UserPermissions)
					.Where(x => x.UserPermissions.Any(y => y.UserId == userId))
					.Select(x => new ServerProfileDtoSimple(
							x.Id,
							x.Name,
							x.ServerType
						))
					.ToListAsync();

				return new(ResultCode.Success, servers);
			}
			catch (Exception)
			{
				return new(ResultCode.Failure);
			}
		}

		public async Task<ServiceResult<ServerProfile>> CreateServer(Guid userId, ServerType serverType, string initialName)
		{
			var user = _dataContext.Users.FirstOrDefault(u => u.Id == userId);
			if (user == null)
				return new(ResultCode.UserNotFound);

			try
			{
				var server = new ServerProfile
				{
					Id = Guid.NewGuid(),
					Name = initialName,
					ServerType = serverType,
				};

				var serverPermission = new ServerPermission
				{
					UserId = userId,
					ServerProfileId = server.Id,
					Permissions = new([ServerPermissions.SuperUser])
				};

				var installPath = Path.Combine(_paths.ServerInstallationsPath, server.Id.ToString(), "install");
				server.InstallationPath = installPath;
				server.UserPermissions.Add(serverPermission);

				_dataContext.Servers.Add(server);
				await _dataContext.SaveChangesAsync();

				Directory.CreateDirectory(installPath);

				return new(ResultCode.Success, server);
			}
			catch (Exception)
			{
				return new(ResultCode.Failure);
			}
		}	
	}
}
