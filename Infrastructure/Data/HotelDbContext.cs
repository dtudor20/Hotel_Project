using Hotel.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Infrastructure.Data;

public class HotelDbContext : IdentityDbContext<ApplicationUser>
{
    public HotelDbContext(DbContextOptions<HotelDbContext> options)
        : base(options)
    {
    }

    public DbSet<Room> Rooms => Set<Room>();
}
