namespace ServerWarden.Api.Models.Database
{
    public class ServerProfile
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ServerType ServerType { get; set; }
        public string InstallationPath { get; set; } = string.Empty;
		public List<ServerPermission> UserPermissions { get; set; } = [];
    }
}
