using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Entities;

public sealed class ApplicationUser : IdentityUser
{
    public bool IsDeleted { get; set; } = false;
}