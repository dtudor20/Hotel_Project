using Hotel.Application.Requests.Queries;
using Hotel.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Application.RequestHandlers.QueryHandlers;

public class GetDashboardStatsQueryHandler : IRequestHandler<GetDashboardStatsQuery, DashboardStats>
{
    private readonly HotelDbContext _dbContext;

    public GetDashboardStatsQueryHandler(HotelDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<DashboardStats> Handle(GetDashboardStatsQuery query, CancellationToken cancellationToken)
    {
        var totalRooms     = await _dbContext.Rooms.CountAsync(cancellationToken);
        var availableRooms = await _dbContext.Rooms.CountAsync(r => r.IsAvailable, cancellationToken);
        var totalUsers     = await _dbContext.Users.CountAsync(cancellationToken);
        var adminUsers     = await _dbContext.Users.CountAsync(u => u.IsAdmin, cancellationToken);

        var bookedRooms = await _dbContext.Rooms
            .Where(r => !r.IsAvailable)
            .ToListAsync(cancellationToken);

        var totalRevenue = bookedRooms.Sum(r => r.PricePerNight);

        // Join with users to attach guest email (left join via GroupJoin).
        var reservedIds = bookedRooms
            .Where(r => r.ReservedByUserId is not null)
            .Select(r => r.ReservedByUserId!)
            .ToList();

        var userLookup = await _dbContext.Users
            .Where(u => reservedIds.Contains(u.Id))
            .ToDictionaryAsync(u => u.Id, u => u.Email, cancellationToken);

        var recent = bookedRooms
            .OrderByDescending(r => r.ReservedAt ?? DateTime.MinValue)
            .Take(8)
            .Select(r => new ReservationInfo(
                r.Id,
                r.RoomNumber,
                r.RoomType,
                r.PricePerNight,
                r.ReservedByUserId is not null && userLookup.TryGetValue(r.ReservedByUserId, out var email)
                    ? email
                    : null,
                r.ReservedAt))
            .ToList();

        return new DashboardStats(
            totalRooms,
            availableRooms,
            totalUsers,
            adminUsers,
            totalRevenue,
            bookedRooms.Count,
            recent);
    }
}
