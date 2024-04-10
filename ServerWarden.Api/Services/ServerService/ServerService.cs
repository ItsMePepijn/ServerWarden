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

		public async Task<ServiceResult<ServerProfileDto>> GetServerProfileById(Guid serverId, Guid userId)
		{
			try
			{
				var server = await _dataContext.Servers
					.Include(x => x.UserPermissions)
					.ThenInclude(x => x.User)
					.FirstOrDefaultAsync(x => x.Id == serverId);

				if (server == null)
					return new(ResultCode.ServerNotFound);
				if (!server.UserPermissions.Any(x => x.UserId == userId))
					return new(ResultCode.UserNotAuthorized);

				var serverDto = new ServerProfileDto(
						server.Id,
						server.Name,
						server.ServerType,
						server.InstallationPath,
						server.UserPermissions
							.Where(x => x.User is not null)
							.Select(x => new ServerUserDto(
									new UserDto(x.User!.Id, x.User.Name),
									x.Permissions
								))
							.ToList()
					);
				return new(ResultCode.Success, serverDto);
			}
			catch (Exception)
			{
				return new(ResultCode.Failure);
			}
		}

		public async Task<ServiceResult<ServerProfileDtoSimple>> CreateServer(Guid userId, ServerType serverType, string initialName)
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

				var installPath = Path.GetFullPath(Path.Combine(_paths.ServerInstallationsPath, server.Id.ToString(), "install"));
				server.InstallationPath = installPath;
				server.UserPermissions.Add(serverPermission);

				_dataContext.Servers.Add(server);
				await _dataContext.SaveChangesAsync();

				Directory.CreateDirectory(installPath);

				return new(ResultCode.Success, new ServerProfileDtoSimple(server.Id, server.Name, server.ServerType));
			}
			catch (Exception)
			{
				return new(ResultCode.Failure);
			}
		}	
	}
}
