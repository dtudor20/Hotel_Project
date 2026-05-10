using Hotel.Application.Requests.Queries;
using Hotel.Domain.Entities;
using Hotel.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Application.RequestHandlers.QueryHandlers;

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, List<ApplicationUser>>
{
    private readonly HotelDbContext _dbContext;

    public GetAllUsersQueryHandler(HotelDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<ApplicationUser>> Handle(GetAllUsersQuery query, CancellationToken cancellationToken)
    {
        return await _dbContext.Users.ToListAsync(cancellationToken);
    }
}
