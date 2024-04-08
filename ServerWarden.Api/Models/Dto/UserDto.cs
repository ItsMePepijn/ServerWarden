namespace ServerWarden.Api.Models.Dto
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsSuperAdmin { get; set; }
	}
}
