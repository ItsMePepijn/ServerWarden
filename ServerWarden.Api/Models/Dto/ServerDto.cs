namespace ServerWarden.Api.Models.Dto
{
	public record CreateServerDto(ServerType ServerType, string Name);
	public record ServerProfileDtoSimple(Guid Id, string Name, ServerType ServerType);
	public record ServerProfileDto(Guid Id, string Name, ServerType ServerType, string InstallationPath, List<ServerUserDto> Users);
	public record ServerUserDto(UserDto User, List<ServerPermissions> Permissions);
}
