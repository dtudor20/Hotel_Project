using Hotel.Application.Requests.Commands;
using Hotel.Domain.Entities;
using Hotel.Infrastructure.Data;
using MediatR;

namespace Hotel.Application.RequestHandlers.CommandHandlers;

public class CreateRoomCommandHandler : IRequestHandler<CreateRoomCommand, Room>
{
    private readonly HotelDbContext _dbContext;

    public CreateRoomCommandHandler(HotelDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Room> Handle(CreateRoomCommand command, CancellationToken cancellationToken)
    {
        var room = new Room
        {
            RoomNumber = command.RoomNumber,
            RoomType = command.RoomType,
            Description = command.Description,
            PricePerNight = command.PricePerNight,
            Capacity = command.Capacity,
            PhotoPath = command.PhotoPath,
            IsAvailable = true
        };

        _dbContext.Rooms.Add(room);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return room;
    }
}
