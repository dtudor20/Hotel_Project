using MediatR;

namespace Hotel.Application.Requests.Queries;

public record DashboardStats(int TotalRooms, int AvailableRooms, int TotalUsers, int AdminUsers);

public record GetDashboardStatsQuery : IRequest<DashboardStats>;
