namespace ServerWarden.Api.Settings
{
	public class Paths
	{
		// Configurable paths
		public string BasePath { get; set; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ServerWarden");

		// SteamCmd
		private string? _steamCmdPath;
		public string SteamCmdPath
		{
			get => _steamCmdPath ??= Path.Combine(BasePath, "steamcmd");
			set => _steamCmdPath = value;
		}

		// Database
		private string? _dbPath;
		public string DbPath
		{
			get => _dbPath ??= Path.Combine(BasePath, "data.db");
			set => _dbPath = value;
		}

		// Non-configurable paths
	}
}
