using IdentityServer.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.DataContexts;

public class AppIdentityDBContext : IdentityDbContext<ApplicationUser>
{
    public AppIdentityDBContext(DbContextOptions<AppIdentityDBContext> options)
        : base(options)
    {
    }
}