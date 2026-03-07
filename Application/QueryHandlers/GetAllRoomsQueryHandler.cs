using Hotel.Application.Queries;
using Hotel.Domain.Entities;
using Hotel.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Application.QueryHandlers;

public class GetAllRoomsQueryHandler
{
    private readonly HotelDbContext _dbContext;

    public GetAllRoomsQueryHandler(HotelDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Room>> Handle(GetAllRoomsQuery query)
    {
        return await _dbContext.Rooms.ToListAsync();
    }
}
