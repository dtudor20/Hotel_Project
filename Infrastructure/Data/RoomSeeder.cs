using Hotel.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Infrastructure.Data;

public static class RoomSeeder
{
    // Room numbers of any previous AurumStay seed set. If the rooms table
    // contains exactly this set (and nothing else), we treat it as the
    // untouched seed and replace it on startup.
    private static readonly string[] LegacySeedRoomNumbers =
        { "101", "102", "201", "202", "301", "302", "401", "402", "501" };

    public static async Task SeedAsync(HotelDbContext db, UserManager<ApplicationUser> userManager)
    {
        var existing = await db.Rooms.ToListAsync();

        var isLegacySeed =
            existing.Count > 0
            && existing.Count <= LegacySeedRoomNumbers.Length
            && existing.All(r => LegacySeedRoomNumbers.Contains(r.RoomNumber));

        if (isLegacySeed)
        {
            db.Rooms.RemoveRange(existing);
            await db.SaveChangesAsync();
            existing = new();
        }

        if (existing.Count > 0) return;

        var alice = await userManager.FindByEmailAsync("alice@example.com");
        var bob   = await userManager.FindByEmailAsync("bob@example.com");
        var clara = await userManager.FindByEmailAsync("clara@example.com");

        // Unsplash CDN — public, stable, no auth. If a URL ever fails,
        // the front-end fallback shows the gold gradient placeholder.
        const string u = "https://images.unsplash.com/photo-";
        const string q = "?auto=format&fit=crop&w=1100&q=80";

        var rooms = new[]
        {
            new Room
            {
                RoomNumber    = "101",
                RoomType      = "Standard",
                Description   = "A calm standard room for one, with a writing desk, soft linens and morning light from the east-facing window.",
                PricePerNight = 220m,
                Capacity      = 1,
                IsAvailable   = true,
                PhotoPath     = $"{u}1631049307264-da0ec9d70304{q}"
            },
            new Room
            {
                RoomNumber    = "102",
                RoomType      = "Standard",
                Description   = "Compact standard room with a city view, blackout curtains and a quiet courtyard side.",
                PricePerNight = 240m,
                Capacity      = 1,
                IsAvailable   = false,
                PhotoPath     = $"{u}1505693416388-ac5ce068fe85{q}",
                ReservedByUserId = bob?.Id,
                ReservedAt    = bob is null ? null : DateTime.UtcNow.AddDays(-3)
            },
            new Room
            {
                RoomNumber    = "201",
                RoomType      = "Standard",
                Description   = "Queen bed, hand-finished oak furniture and a marble bathroom with rain shower.",
                PricePerNight = 340m,
                Capacity      = 2,
                IsAvailable   = true,
                PhotoPath     = $"{u}1611892440504-42a792e24d32{q}"
            },
            new Room
            {
                RoomNumber    = "202",
                RoomType      = "Standard",
                Description   = "Twin beds, garden-side balcony and a small reading nook by the window.",
                PricePerNight = 360m,
                Capacity      = 2,
                IsAvailable   = false,
                PhotoPath     = $"{u}1566665797739-1674de7a421a{q}",
                ReservedByUserId = alice?.Id,
                ReservedAt    = alice is null ? null : DateTime.UtcNow.AddDays(-1)
            },
            new Room
            {
                RoomNumber    = "301",
                RoomType      = "Standard",
                Description   = "Family-friendly standard with a queen and a daybed, perfect for a small family or a couple with a child.",
                PricePerNight = 460m,
                Capacity      = 3,
                IsAvailable   = true,
                PhotoPath     = $"{u}1540518614846-7eded433c457{q}"
            },
            new Room
            {
                RoomNumber    = "401",
                RoomType      = "Deluxe",
                Description   = "King bed, lounge area and a deep soaking tub overlooking the bay. Breakfast included.",
                PricePerNight = 580m,
                Capacity      = 2,
                IsAvailable   = false,
                PhotoPath     = $"{u}1582719478250-c89cae4dc85b{q}",
                ReservedByUserId = clara?.Id,
                ReservedAt    = clara is null ? null : DateTime.UtcNow.AddHours(-6)
            },
            new Room
            {
                RoomNumber    = "402",
                RoomType      = "Deluxe",
                Description   = "Quiet corner deluxe with floor-to-ceiling windows, a private espresso bar and a writing alcove.",
                PricePerNight = 720m,
                Capacity      = 3,
                IsAvailable   = true,
                PhotoPath     = $"{u}1590490360182-c33d57733427{q}"
            },
            new Room
            {
                RoomNumber    = "501",
                RoomType      = "Deluxe",
                Description   = "Top-floor deluxe with a private terrace, butler service on request and a curated minibar.",
                PricePerNight = 920m,
                Capacity      = 4,
                IsAvailable   = true,
                PhotoPath     = $"{u}1551882547-ff40c63fe5fa{q}"
            }
        };

        await db.Rooms.AddRangeAsync(rooms);
        await db.SaveChangesAsync();
    }
}
