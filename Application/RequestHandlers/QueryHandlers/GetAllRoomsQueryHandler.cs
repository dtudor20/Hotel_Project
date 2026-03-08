using Hotel.Application.Requests.Queries;
using Hotel.Domain.Entities;
using Hotel.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Application.RequestHandlers.QueryHandlers;

public class GetAllRoomsQueryHandler : IRequestHandler<GetAllRoomsQuery, List<Room>>
{
    private readonly HotelDbContext _dbContext;

    public GetAllRoomsQueryHandler(HotelDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Room>> Handle(GetAllRoomsQuery query, CancellationToken cancellationToken)
    {
        return await _dbContext.Rooms.ToListAsync(cancellationToken);
    }
}
