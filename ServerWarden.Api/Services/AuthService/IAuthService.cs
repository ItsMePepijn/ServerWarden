using ServerWarden.Api.Models;
using ServerWarden.Api.Models.Dto;
using System.Security.Claims;

namespace ServerWarden.Api.Services.AuthService
{
    public interface IAuthService
	{
		Task<ServiceResult<string?>> Register(UserLoginDto userDto);
		ServiceResult<string?> Login(UserLoginDto userDto);

		IEnumerable<Claim> ParseClaimsFromJwt(string jwt);
	}
}
