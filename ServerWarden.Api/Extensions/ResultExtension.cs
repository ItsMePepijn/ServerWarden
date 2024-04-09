using Microsoft.AspNetCore.Mvc;
using ServerWarden.Api.Models;
using System.Text.Json;

namespace ServerWarden.Api.Extensions
{
    public static class ResultExtension
    {
        public static IResult ToResponse<T>(this ServiceResult<T> result)
        {
            if (result.Success)
            {
                return TypedResults.Ok(result);
            }

            return result.Code switch
            {
                ResultCode.InvalidParameters
                or ResultCode.InvalidNewPassword
                or ResultCode.InvalidNewUsername
                or ResultCode.InvalidPassword
                or ResultCode.UserExists
                or ResultCode.UserNotFound
				or ResultCode.ServerNotFound
                or ResultCode.UserNotAuthorized 
				    => TypedResults.BadRequest(result),
                _ => new InternalServerErrorObjectResult<ServiceResult<T>>(result)
            };

        }

        public static IResult ToResponse(this ServiceResult result)
        {
            if (result.Success)
            {
                return TypedResults.Ok(result);
            }

            return result.Code switch
            {
                ResultCode.InvalidParameters
                or ResultCode.InvalidNewPassword
                or ResultCode.InvalidNewUsername
                or ResultCode.InvalidPassword
                or ResultCode.UserExists
                or ResultCode.UserNotFound
				or ResultCode.ServerNotFound
				or ResultCode.UserNotAuthorized
					=> TypedResults.BadRequest(result),
                _ => new InternalServerErrorObjectResult<ServiceResult>(result)
            };

        }
    }

    public class InternalServerErrorObjectResult<T>(T value) : IResult
    {
        private readonly T _value = value;

        public async Task ExecuteAsync(HttpContext httpContext)
        {
            httpContext.Response.StatusCode = 500;
            httpContext.Response.ContentType = "application/json";

            await httpContext.Response.WriteAsJsonAsync(_value);
        }
    }
}
