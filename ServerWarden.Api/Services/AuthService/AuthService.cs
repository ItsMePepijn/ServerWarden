using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ServerWarden.Api.Models;
using ServerWarden.Api.Models.Database;
using ServerWarden.Api.Models.Dto;
using ServerWarden.Api.Settings;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;

namespace ServerWarden.Api.Services.AuthService
{
    public class AuthService(
		IOptions<Keys> keys,
		DataContext dataContext
		) : IAuthService
	{
		private readonly Keys _keys = keys.Value;
		private readonly DataContext _dataContext = dataContext;

		public async Task<ServiceResult<string?>> Register(UserLoginDto userDto)
		{
			// Validate user info
			if (string.IsNullOrWhiteSpace(userDto.Name) || string.IsNullOrWhiteSpace(userDto.Password))
				return new(ResultCode.InvalidParameters);
			if(userDto.Name.Length < 3 || userDto.Name.Length > 20)
				return new(ResultCode.InvalidNewUsername);
			if(userDto.Password.Length < 5)
				return new(ResultCode.InvalidNewPassword);

			// Checks if username already exists
			User? dbUser = _dataContext.Users.FirstOrDefault(user => user.Name == userDto.Name);
			if (dbUser is not null)
				return new(ResultCode.UserExists);

			// Hash password and create a new user object
			var passwordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
			User NewUser = new()
			{
				Name = userDto.Name,
				PasswordHash = passwordHash,
			};

			_dataContext.Add(NewUser);
			await _dataContext.SaveChangesAsync();

			var user = new User
			{
				Id = NewUser.Id,
				Name = NewUser.Name
			};

			return new(ResultCode.Success, GenerateToken(user));
		}

		public ServiceResult<string?> Login(UserLoginDto userDto)
		{
			try
			{
				User? dbUser;
				// Verify user info
				dbUser = _dataContext.Users.SingleOrDefault(user => user.Name == userDto.Name);
				if (dbUser is null || !BCrypt.Net.BCrypt.Verify(userDto.Password, dbUser.PasswordHash))
				{
					return new(ResultCode.InvalidPassword);
				}
				
				return new(ResultCode.Success, GenerateToken(dbUser));
			}
			catch (Exception)
			{
				return new(ResultCode.Failure);
			}
		}

		public IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
		{
			string payload = jwt.Split('.')[1];
			byte[] jsonBytes = ParseBase64WithoutPadding(payload);
			var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes)!;

			return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()!));
		}

		private static byte[] ParseBase64WithoutPadding(string base64)
		{
			switch (base64.Length % 4)
			{
				case 2: base64 += "=="; break;
				case 3: base64 += "="; break;
			}
			return Convert.FromBase64String(base64);
		}

		private string GenerateToken(User user)
		{
			List<Claim> claims =
			[
				new Claim("id", user.Id.ToString()),
				new Claim("name", user.Name),
				new Claim("isSuperAdmin", user.IsSuperAdmin.ToString())
			];

			SymmetricSecurityKey key = new(System.Text.Encoding.UTF8.GetBytes(_keys.JwtKey));

			SigningCredentials cred = new(key, SecurityAlgorithms.HmacSha512Signature);

			JwtSecurityToken token = new(
				claims: claims,
				expires: DateTime.Now.AddMonths(1),
				signingCredentials: cred
				);

			var jwt = new JwtSecurityTokenHandler().WriteToken(token);

			return jwt;
		}
	}
}
