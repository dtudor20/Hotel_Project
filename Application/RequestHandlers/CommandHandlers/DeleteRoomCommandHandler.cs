using Hotel.Application.Requests.Commands;
using Hotel.Infrastructure.Data;
using MediatR;

namespace Hotel.Application.RequestHandlers.CommandHandlers;

public class DeleteRoomCommandHandler : IRequestHandler<DeleteRoomCommand, bool>
{
    private readonly HotelDbContext _dbContext;

    public DeleteRoomCommandHandler(HotelDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> Handle(DeleteRoomCommand command, CancellationToken cancellationToken)
    {
        var room = await _dbContext.Rooms.FindAsync([command.Id], cancellationToken);
        if (room is null) return false;

        _dbContext.Rooms.Remove(room);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}
