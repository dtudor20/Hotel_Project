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
        var totalRooms = await _dbContext.Rooms.CountAsync(cancellationToken);
        var availableRooms = await _dbContext.Rooms.CountAsync(r => r.IsAvailable, cancellationToken);
        var totalUsers = await _dbContext.Users.CountAsync(cancellationToken);
        var adminUsers = await _dbContext.Users.CountAsync(u => u.IsAdmin, cancellationToken);

        return new DashboardStats(totalRooms, availableRooms, totalUsers, adminUsers);
    }
}
