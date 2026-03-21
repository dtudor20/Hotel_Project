using Microsoft.AspNetCore.Identity;

namespace Hotel.Domain.Entities;

public class ApplicationUser : IdentityUser
{
    public bool IsAdmin { get; set; }
}
