using Hotel.Application.Requests.Queries;
using MediatR;

namespace Hotel.Application.Endpoints;

public static class RoomEndpoints
{
    public static void MapRoomEndpoints(this WebApplication app)
    {
        app.MapGet("/api/rooms", async (ISender sender) =>
        {
            var rooms = await sender.Send(new GetAllRoomsQuery());
            return Results.Ok(rooms);
        });
    }
}
