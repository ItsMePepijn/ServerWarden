using ServerWarden.Api.Extensions;
using ServerWarden.Api.Models.Dto;
using ServerWarden.Api.Services.AuthService;

namespace ServerWarden.Api.RouteHandlers
{
    public static class AuthHandler
    {
        public static RouteGroupBuilder MapAuthRoutes(this RouteGroupBuilder builder)
        {
            builder.MapPost("/login", Login);
            builder.MapPost("/register", Register);

            return builder;
        }

        private static IResult Login(UserLoginDto userLogin, IAuthService authService)
        {
            var result = authService.Login(userLogin);
            return result.ToResponse();
        }

        private static async Task<IResult> Register(UserLoginDto userLogin, IAuthService authService)
        {
            var result = await authService.Register(userLogin);
            return result.ToResponse();
        }
    }
}
