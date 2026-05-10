using Hotel.Domain.Entities;
using MediatR;

namespace Hotel.Application.Requests.Commands;

public record CreateRoomCommand(
    string RoomNumber,
    string RoomType,
    string Description,
    decimal PricePerNight,
    int Capacity,
    string? PhotoPath
) : IRequest<Room>;
