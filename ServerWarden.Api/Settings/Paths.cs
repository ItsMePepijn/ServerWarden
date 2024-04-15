namespace ServerWarden.Api.Settings
{
	public class Paths
	{
		// Configurable paths
		private string? _basePath;
		public string BasePath
		{
			get => Path.GetFullPath(_basePath ??= Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ServerWarden"));
			set => _basePath = value;
		}

		// SteamCmd
		private string? _steamCmdPath;
		public string SteamCmdPath
		{
			get => Path.GetFullPath(_steamCmdPath ??= Path.Combine(BasePath, "steamcmd"));
			set => _steamCmdPath = value;
		}

		// Database
		private string? _dbPath;
		public string DbPath
		{
			get => Path.GetFullPath(_dbPath ??= Path.Combine(BasePath, "data.db"));
			set => _dbPath = value;
		}

		// Hangfire Database
		private string? _hangfireDbPath;
		public string HangfireDbPath
		{
			get => Path.GetFullPath(_hangfireDbPath ??= Path.Combine(BasePath, "hangfire.db"));
			set => _hangfireDbPath = value;
		}

		// Server installations
		private string? _serverInstallationsPath;
		public string ServerInstallationsPath
		{
			get => Path.GetFullPath(_serverInstallationsPath ??= Path.Combine(BasePath, "servers"));
			set => _serverInstallationsPath = value;
		}

		// Logfiles
		private string? _logPath;
		public string LogPath
		{
			get => Path.GetFullPath(_logPath ??= Path.Combine(BasePath, "log", "serverwarden.log"));
			set => _logPath = value;
		}

		// Non-configurable paths
	}
}
