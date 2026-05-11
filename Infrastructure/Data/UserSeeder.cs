using Hotel.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Hotel.Infrastructure.Data;

public static class UserSeeder
{
    public record SeedAccount(string Email, string Password, bool IsAdmin);

    public static readonly SeedAccount[] Accounts =
    {
        new("admin@aurumstay.local",  "Admin#2026",   true),
        new("alice@example.com",      "Alice#2026",   false),
        new("bob@example.com",        "Bob#2026",     false),
        new("clara@example.com",      "Clara#2026",   false)
    };

    public static async Task SeedAsync(UserManager<ApplicationUser> userManager)
    {
        foreach (var account in Accounts)
        {
            if (await userManager.FindByEmailAsync(account.Email) is not null) continue;

            var user = new ApplicationUser
            {
                UserName       = account.Email,
                Email          = account.Email,
                EmailConfirmed = true,
                IsAdmin        = account.IsAdmin
            };

            await userManager.CreateAsync(user, account.Password);
        }
    }
}
