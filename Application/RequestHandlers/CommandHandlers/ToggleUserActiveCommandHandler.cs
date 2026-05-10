using Hotel.Application.Requests.Commands;
using Hotel.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Hotel.Application.RequestHandlers.CommandHandlers;

public class ToggleUserActiveCommandHandler : IRequestHandler<ToggleUserActiveCommand, bool>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public ToggleUserActiveCommandHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<bool> Handle(ToggleUserActiveCommand command, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(command.UserId);
        if (user is null) return false;

        var isCurrentlyLocked = user.LockoutEnd.HasValue && user.LockoutEnd > DateTimeOffset.UtcNow;

        if (isCurrentlyLocked)
        {
            await _userManager.SetLockoutEndDateAsync(user, null);
        }
        else
        {
            await _userManager.SetLockoutEnabledAsync(user, true);
            await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddYears(100));
        }

        return true;
    }
}
