using Hangfire;
using Hangfire.Storage.SQLite;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ServerWarden.Api;
using ServerWarden.Api.Hubs;
using ServerWarden.Api.Middleware;
using ServerWarden.Api.RouteHandlers;
using ServerWarden.Api.Services.AuthService;
using ServerWarden.Api.Services.ServerService;
using ServerWarden.Api.Settings;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configure application as a Windows service
builder.Services.AddWindowsService(options =>
{
	options.ServiceName = "ServerWarden";
});

// Swagger configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
	option.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
	{
		In = ParameterLocation.Header,
		Description = "Please enter a valid token",
		Name = "Authorization",
		Type = SecuritySchemeType.Http,
		BearerFormat = "JWT",
		Scheme = "Bearer"
	});
	option.OperationFilter<SecurityRequirementsOperationFilter>();
});

// Settings and constants
builder.Services.Configure<Paths>(builder.Configuration.GetSection("Paths"));
builder.Services.Configure<Keys>(builder.Configuration.GetSection("Keys"));

// Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IServerService, ServerService>();
builder.Services.AddScoped<ServerHubService>();

// SignalR
string[] mimeTypes = ["application/octet-stream"];
builder.Services.AddSignalR();
builder.Services.AddResponseCompression(options =>
{
	options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(mimeTypes);
});

// Database
builder.Services.AddDbContext<DataContext>((serviceProvider, options) =>
{
	var paths = serviceProvider.GetRequiredService<IOptions<Paths>>().Value;
	options.UseSqlite("Data Source=" + paths.DbPath);
});

// Authentication
builder.Services.AddAuthentication().AddJwtBearer(options =>
{
	options.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuerSigningKey = true,
		ValidateAudience = false,
		ValidateIssuer = false,
		IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
				builder.Configuration.GetSection("Keys:JwtKey").Value!))
	};

	options.Events = new JwtBearerEvents
	{
		OnMessageReceived = context =>
		{
			var accessToken = context.Request.Query["access_token"];

			var path = context.HttpContext.Request.Path;
			if (!string.IsNullOrEmpty(accessToken) &&
				(path.StartsWithSegments("/serverHub")))
			{
				context.Token = accessToken;
			}
			return Task.CompletedTask;
		}
	};
});

// Hangfire
string[] hangfireQueues = ["installation", "default"];
builder.Services.AddHangfire((serviceProvider, options) =>
{
	var paths = serviceProvider.GetRequiredService<IOptions<Paths>>().Value;

	options.UseSimpleAssemblyNameTypeSerializer()
		.UseRecommendedSerializerSettings()
		.UseSQLiteStorage(paths.HangfireDbPath);
});
builder.Services.AddHangfireServer(options =>
{
	options.Queues = hangfireQueues;
});

builder.Services.AddAuthorization();

// Log4Net
var serviceProvider = builder.Services.BuildServiceProvider();
using var builderScope = serviceProvider.CreateScope();
var logPath = builderScope.ServiceProvider.GetRequiredService<IOptions<Paths>>().Value;

log4net.GlobalContext.Properties["LogPath"] = logPath.LogPath;
builder.Logging.AddLog4Net();


var app = builder.Build();

// Ensure database is created and up to date
using var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
dbContext.Database.Migrate();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
	app.UseHangfireDashboard();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseUserIdProvider();

// Routes
app.MapGroup("/auth")
	.MapAuthRoutes()
	.WithOpenApi();

app.MapGroup("/servers")
	.MapServerRoutes()
	.RequireAuthorization()
	.WithOpenApi();

// SignalR Hubs
app.MapHub<ServerHub>("/serverHub")
	.RequireAuthorization();

app.Run();