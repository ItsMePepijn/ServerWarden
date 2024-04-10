namespace ServerWarden.Api.Models.Database
{
	public class ServerPermission
	{
		public Guid UserId { get; set; }
		public Guid ServerProfileId { get; set; }
		public User? User { get; set; }
		public List<ServerPermissions> Permissions { get; set; } = [];
	}
}
