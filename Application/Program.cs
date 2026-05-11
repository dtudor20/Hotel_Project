using Hotel.Application.Extensions;
using Hotel.Domain.Entities;
using Hotel.Infrastructure.Data;
using Hotel.Presentation;
using Hotel.Presentation.Endpoints;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme)
    .AddIdentityCookies();
builder.Services.AddAuthorization();

builder.Services
    .AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<HotelDbContext>();
    await dbContext.Database.MigrateAsync();
    await IdentitySchemaInitializer.EnsureIdentitySchemaAsync(dbContext);

    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    await UserSeeder.SeedAsync(userManager);
    await RoomSeeder.SeedAsync(dbContext, userManager);
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapAccountEndpoints();
app.MapRazorComponents<App>()
   .AddInteractiveServerRenderMode();

app.Run();
