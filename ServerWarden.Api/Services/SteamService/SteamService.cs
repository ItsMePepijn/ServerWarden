using Microsoft.Extensions.Options;
using ServerWarden.Api.Settings;
using SteamCMD.ConPTY;

namespace ServerWarden.Api.Services.SteamService
{
	public class SteamService(ILogger<SteamService> logger, IOptions<Paths> paths) : ISteamService
	{
		private readonly Paths _paths = paths.Value;
		private readonly ILogger<SteamService> _logger = logger;

		public void InstallArkSurvivalEvolved(string path)
		{
			InstallGameFromSteam(SteamGameId.ArkSurvivalEvolved, path);
		}

		private void InstallGameFromSteam(SteamGameId gameId, string installLocation)
		{
			var steamCmd = new SteamCMDConPTY
			{
				WorkingDirectory = _paths.SteamCmdPath,
				Arguments = $"+force_install_dir {installLocation} +login anonymous +app_update {gameId} +quit"
			};

			steamCmd.OutputDataReceived += (sender, data) => _logger.LogInformation(data);

			steamCmd.Start();
		}
	}
}
