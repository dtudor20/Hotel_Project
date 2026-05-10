using MediatR;

namespace Hotel.Application.Requests.Commands;

public record DeleteRoomCommand(int Id) : IRequest<bool>;
