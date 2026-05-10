using MediatR;

namespace Hotel.Application.Requests.Commands;

public record UpdateRoomCommand(
    int Id,
    string RoomNumber,
    string RoomType,
    string Description,
    decimal PricePerNight,
    int Capacity,
    bool IsAvailable,
    string? PhotoPath
) : IRequest<bool>;
