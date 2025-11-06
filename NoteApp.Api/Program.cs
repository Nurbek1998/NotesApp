using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using NoteApp.Api.Data;
using NoteApp.Api.Domain.Entities;
using NoteApp.Api.Interfaces;
using NoteApp.Api.Middlewares;
using NoteApp.Api.OpenApi;
using NoteApp.Api.Repositories;
using NoteApp.Api.Services;
using NoteApp.Api.UnitOfWork;
using Scalar.AspNetCore;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddDbContext<AppDbContext>(options =>
options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var loggerFactory = LoggerFactory.Create(logging =>
{
	logging.AddConsole();
});

var mapperConfig = new MapperConfiguration(
	cfg =>
	{
		cfg.AddMaps(AppDomain.CurrentDomain.GetAssemblies());
	},
	loggerFactory
);
builder.Services.AddSingleton<IMapper>(mapperConfig.CreateMapper());

builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<INoteRepository, NoteRepository>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<INoteService, NoteService>();

builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserContext, CurrentUserContext>();

builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}
)
.AddJwtBearer(options =>
{
	var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY")
		?? throw new InvalidOperationException("JWT_KEY environment variable not set.");

	var key = Encoding.UTF8.GetBytes(jwtKey);
	options.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuer = true,
		ValidateAudience = true,
		ValidateLifetime = true,
		ValidateIssuerSigningKey = true,
		ValidIssuer = builder.Configuration["Jwt:Issuer"],
		ValidAudience = builder.Configuration["Jwt:Audience"],
		IssuerSigningKey = new SymmetricSecurityKey(key)
	};
});

builder.Services.AddAuthorizationBuilder()
	.AddPolicy("AdminPolicy", policy =>
	{
		policy.RequireAuthenticatedUser();
		policy.RequireRole("admin");
	})
	.AddPolicy("UserPolicy", policy =>
	{
		policy.RequireAuthenticatedUser();
		policy.RequireRole("user");
	})
	.AddPolicy("AdminOrUserPolicy", policy =>
	{
		policy.RequireAuthenticatedUser();
		policy.RequireRole("admin", "user");
	});

builder.Services.AddOpenApi(options =>
{
	options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
	app.MapScalarApiReference(options =>
	{
		options.Theme = ScalarTheme.BluePlanet;
	}
	);
}

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionMiddleware>();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
