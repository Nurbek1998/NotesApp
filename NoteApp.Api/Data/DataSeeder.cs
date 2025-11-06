using Microsoft.AspNetCore.Identity;
using NoteApp.Api.Domain.Entities;
using NoteApp.Api.Interfaces;

namespace NoteApp.Api.Data;
public static class DataSeeder
{
	public static async Task SeedAsync(IServiceProvider serviceProvider)
	{
		using var scope=serviceProvider.CreateScope();
		var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
		var hasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<User>>();
		var	jwtService=scope.ServiceProvider.GetRequiredService<IJwtService>();

		if(!context.Users.Any())
		{
			var users=new List<User>();

			var userNames = new[]
		   {
				"Jasur", "Malika", "Azizbek", "Dilnoza", "Bekzod",
				"Nodira", "Ulug‘bek", "Laylo", "Zafar", "Madina"
			};

			var adminNames = new[]
			{
				"Kamoliddin", "Nurbek", "Asliddin", "Gulbahor", "Sherzod"
			};

			foreach (var name in userNames)
			{
				var email = $"{name.ToLower()}@mail.com";
				var user = new User
				{
					Username = name,
					Email = email,
					Role = "user",
					CreatedAt = DateTime.UtcNow
				};
				user.Password = hasher.HashPassword(user, "User123!");
				users.Add(user);
			}

			foreach (var name in adminNames)
			{
				var email = $"{name.ToLower()}@admin.com";
				var admin = new User
				{
					Username = name,
					Email = email,
					Role = "admin",
					CreatedAt = DateTime.UtcNow
				};
				admin.Password = hasher.HashPassword(admin, "Admin123!");
				users.Add(admin);
			}

			await context.Users.AddRangeAsync(users);
			await context.SaveChangesAsync();

			Console.WriteLine("✅ Fake foydalanuvchilar va adminlar muvaffaqiyatli yaratildi.\n");

			// Har bir user uchun token yaratamiz
			foreach (var user in users)
			{
				var token = jwtService.GenerateToken(user);
				Console.WriteLine($"{user.Role.ToUpper()} | {user.Username} | {user.Email} | Token: {token}\n");
			}

			Console.WriteLine("🔥 Barcha tokenlar muvaffaqiyatli generatsiya qilindi.\n");
		}
		else
		{
			Console.WriteLine("ℹ️ Users jadvalida allaqachon ma’lumotlar mavjud. Seeding o‘tkazib yuborildi.");
		}
	}

	}

