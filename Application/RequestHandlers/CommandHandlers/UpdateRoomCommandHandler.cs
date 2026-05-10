using Hotel.Application.Requests.Commands;
using Hotel.Infrastructure.Data;
using MediatR;

namespace Hotel.Application.RequestHandlers.CommandHandlers;

public class UpdateRoomCommandHandler : IRequestHandler<UpdateRoomCommand, bool>
{
    private readonly HotelDbContext _dbContext;

    public UpdateRoomCommandHandler(HotelDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> Handle(UpdateRoomCommand command, CancellationToken cancellationToken)
    {
        var room = await _dbContext.Rooms.FindAsync([command.Id], cancellationToken);
        if (room is null) return false;

        room.RoomNumber = command.RoomNumber;
        room.RoomType = command.RoomType;
        room.Description = command.Description;
        room.PricePerNight = command.PricePerNight;
        room.Capacity = command.Capacity;
        room.IsAvailable = command.IsAvailable;

        if (command.PhotoPath is not null)
            room.PhotoPath = command.PhotoPath;

        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}
