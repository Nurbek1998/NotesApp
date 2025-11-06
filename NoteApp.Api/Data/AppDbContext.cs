using Microsoft.EntityFrameworkCore;
using NoteApp.Api.Domain.Entities;

namespace NoteApp.Api.Data;
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
	public DbSet<User> Users { get; set; }
	public DbSet<Note> Notes { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Note>()
			.HasOne(u => u.User)
			.WithMany(n => n.Notes)
			.HasForeignKey(n => n.UserId)
			.OnDelete(DeleteBehavior.Cascade);

		modelBuilder.Entity<User>()
			.Property(u => u.Id)
			.HasDefaultValueSql("gen_random_uuid()");

		base.OnModelCreating(modelBuilder);
	}
}
