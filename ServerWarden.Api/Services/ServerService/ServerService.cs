using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ServerWarden.Api.Hubs;
using ServerWarden.Api.Models;
using ServerWarden.Api.Models.Database;
using ServerWarden.Api.Models.Dto;
using ServerWarden.Api.Settings;
using SteamCMD.ConPTY;
using System.Diagnostics;

namespace ServerWarden.Api.Services.ServerService
{
    public class ServerService(DataContext dataContext, ServerHubService serverHub, ILogger<ServerService> logger, IOptions<Paths> paths) : IServerService
	{
		private readonly DataContext _dataContext = dataContext;
		private readonly ServerHubService _serverHub = serverHub;
		private readonly ILogger<ServerService> _logger = logger;
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
						server.HasBeenInstalled,
						server.ShouldBeInstalling,
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

				var installPath = Path.Combine(_paths.ServerInstallationsPath, server.Id.ToString(), "install");
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

		public async Task<ServiceResult> InstallServer(Guid serverId, Guid userId)
		{
			var server = await _dataContext.Servers
				.Include(x => x.UserPermissions)
				.FirstOrDefaultAsync(x => x.Id == serverId);

			if (server == null)
				return new(ResultCode.ServerNotFound);
			if (!server.UserPermissions.Any(x => x.UserId == userId && x.Permissions.Contains(ServerPermissions.SuperUser)))
				return new(ResultCode.UserNotAuthorized);
			if (server.HasBeenInstalled)
				return new(ResultCode.ServerAlreadyInstalled);
			if (server.ShouldBeInstalling)
				return new(ResultCode.ServerIsInstalling);

			try
			{
				server.ShouldBeInstalling = true;
				await _dataContext.SaveChangesAsync();

				_serverHub.ServerUpdated(serverId);
				_serverHub.ServerInstallLog(serverId, "Server started installing");

				switch (server.ServerType)
				{
					case ServerType.ArkSurvivalEvolved:
						BackgroundJob.Enqueue(() => InstallGameFromSteam(SteamGameId.ArkSurvivalEvolved, server.InstallationPath, server.Id));
						break;
					default:
						return new(ResultCode.InvalidServerType);
				}

				return new(ResultCode.Success);
			}
			catch (Exception e)
			{
				_logger.LogError(e, "Failed to install server");
				return new(ResultCode.Failure);
			}
		}

		public async Task<ServiceResult> StartServer(Guid serverId, Guid userId)
		{
			var server = await _dataContext.Servers
				.Include(x => x.UserPermissions)
				.FirstOrDefaultAsync(x => x.Id == serverId);

			if (server == null)
				return new(ResultCode.ServerNotFound);
			if (!UserHasPermission(server, userId, ServerPermissions.Start))
				return new(ResultCode.UserNotAuthorized);
			if (!server.HasBeenInstalled)
				return new(ResultCode.ServerNotInstalled);

			try
			{
				server.ShouldBeRunning = true;
				await _dataContext.SaveChangesAsync();

				_serverHub.ServerUpdated(serverId);

				return StartServerInternal(server);
			}
			catch (Exception e)
			{
				_logger.LogError(e, "Failed to start server");
				return new(ResultCode.Failure);
			}
		}

		public async Task StartServer(Guid serverId)
		{
			var server = await _dataContext.Servers
				.FirstOrDefaultAsync(x => x.Id == serverId) ?? throw new Exception("Server not found");
			if (!server.HasBeenInstalled)
				throw new Exception("Server not installed");

			var result = StartServerInternal(server);

			if (!result.Success)
				throw new Exception(result.Code.ToString());
		}

		private static ServiceResult StartServerInternal(ServerProfile server)
		{
			switch (server.ServerType)
			{
				case ServerType.ArkSurvivalEvolved:
					Process.Start(Path.Combine(server.InstallationPath, "ShooterGame", "Binaries", "Win64", "ShooterGameServer.exe"));
					break;
				default:
					return new(ResultCode.InvalidServerType);
			}

			return new(ResultCode.Success);
		}

		private static bool UserHasPermission(ServerProfile server, Guid userId, params ServerPermissions[] permissions)
		{
			return server.UserPermissions.Any(x => x.UserId == userId && (x.Permissions.Contains(ServerPermissions.SuperUser) || permissions.Any(p => x.Permissions.Contains(p))));
		}

		public async Task ServerFinishedInstalling(Guid serverId)
		{
			var server = await _dataContext.Servers
				.FirstOrDefaultAsync(x => x.Id == serverId);

			if (server == null)
				return;

			server.ShouldBeInstalling = false;
			server.HasBeenInstalled = true;
			await _dataContext.SaveChangesAsync();

			_serverHub.ServerUpdated(serverId);
		}

		//Background tasks
		[Queue("installation")]
		public async Task InstallGameFromSteam(SteamGameId gameId, string installLocation, Guid serverId)
		{
			var steamCmd = new SteamCMDConPTY
			{
				WorkingDirectory = _paths.SteamCmdPath,
				Arguments = $"+force_install_dir {installLocation} +login anonymous +app_update {(int)gameId} +quit"
			};

			var tcs = new TaskCompletionSource<bool>();

			steamCmd.OutputDataReceived += (sender, data) =>
			{
				if(string.IsNullOrWhiteSpace(data))
					return;

				_logger.LogInformation("{data}", data);
				_serverHub.ServerInstallLog(serverId, data);
			};
			steamCmd.Exited += async (sender, args) =>
			{
				_logger.LogInformation("Finished installing {gameId} server", gameId);
				await ServerFinishedInstalling(serverId);
				tcs.SetResult(true);
			};

			steamCmd.Start();

			// Correctly exit method when steamcmd is done
			await tcs.Task;
		}
	}
}
