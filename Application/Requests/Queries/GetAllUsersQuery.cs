using Hotel.Domain.Entities;
using MediatR;

namespace Hotel.Application.Requests.Queries;

public record GetAllUsersQuery : IRequest<List<ApplicationUser>>;
