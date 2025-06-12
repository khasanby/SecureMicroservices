using IdentityServer.DataContexts;
using IdentityServer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Duende.IdentityServer.EntityFramework.DbContexts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("IdentityDBConnectionString");

builder.Services.AddDbContext<AppIdentityDBContext>(options =>
{
    options.UseNpgsql(connectionString);
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AppIdentityDBContext>()
                .AddDefaultTokenProviders();

builder.Services.AddIdentityServer()
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = builder =>
                    {
                        builder.UseNpgsql(connectionString, npgsqlOptions =>
                        {
                            npgsqlOptions.MigrationsAssembly(typeof(Program).Assembly.FullName);
                        });
                    };
                })
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = builder =>
                    {
                        builder.UseNpgsql(connectionString, npgsqlOptions =>
                        {
                            npgsqlOptions.MigrationsAssembly(typeof(Program).Assembly.FullName);
                        });
                    };
                    options.EnableTokenCleanup = true;
                    options.TokenCleanupInterval = 30; // Cleanup every 30 minutes
                })
                .AddAspNetIdentity<ApplicationUser>()
                .AddDeveloperSigningCredential(); // For development purposes only, use a real certificate in production

var app = builder.Build();

using var scope = app.Services.CreateScope();
var serviceProvider = scope.ServiceProvider;
// Apply migrations and seed the database
IdentityServerDbSeeder.Seed(serviceProvider);

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseRouting();
app.UseIdentityServer();

app.Run();
