using Hotel.Application.QueryHandlers;
using Hotel.Application.Queries;

namespace Hotel.Application.Endpoints;

public static class RoomEndpoints
{
    public static void MapRoomEndpoints(this WebApplication app)
    {
        app.MapGet("/api/rooms", async (GetAllRoomsQueryHandler handler) =>
        {
            var rooms = await handler.Handle(new GetAllRoomsQuery());
            return Results.Ok(rooms);
        });
    }
}
