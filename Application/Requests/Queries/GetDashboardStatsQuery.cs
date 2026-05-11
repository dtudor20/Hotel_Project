using MediatR;

namespace Hotel.Application.Requests.Queries;

public record ReservationInfo(
    int RoomId,
    string RoomNumber,
    string RoomType,
    decimal PricePerNight,
    string? GuestEmail,
    DateTime? ReservedAt
);

public record DashboardStats(
    int TotalRooms,
    int AvailableRooms,
    int TotalUsers,
    int AdminUsers,
    decimal TotalRevenue,
    int ReservationCount,
    IReadOnlyList<ReservationInfo> RecentReservations
);

public record GetDashboardStatsQuery : IRequest<DashboardStats>;
