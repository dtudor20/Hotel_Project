using MediatR;

namespace Hotel.Application.Requests.Commands;

public record ToggleUserActiveCommand(string UserId) : IRequest<bool>;
