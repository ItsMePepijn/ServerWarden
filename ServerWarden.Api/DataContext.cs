using Microsoft.EntityFrameworkCore;
using ServerWarden.Api.Models;
using ServerWarden.Api.Models.Database;

namespace ServerWarden.Api
{
    public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
	{
		public DbSet<User> Users { get; set; }
		public DbSet<ServerProfile> Servers { get; set; }
		public DbSet<ServerPermission> ServerPermissions { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<ServerPermission>()
				.HasKey(b => new { b.UserId, b.ServerProfileId });
		}
	}
}