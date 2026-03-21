using System.ComponentModel.DataAnnotations;
using Hotel.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Presentation.Endpoints;

public static class AccountEndpoints
{
    public static IEndpointRouteBuilder MapAccountEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/account");

        group.MapPost("/login", LoginAsync);
        group.MapPost("/logout", LogoutAsync);

        return endpoints;
    }

    private static async Task<IResult> LoginAsync(
        [FromForm] LoginRequest request,
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager)
    {
        if (!MiniValidator.TryValidate(request))
        {
            return TypedResults.Redirect($"/login?error={Uri.EscapeDataString("Please enter a valid email and password.")}");
        }

        var user = await userManager.FindByEmailAsync(request.Email);

        if (user is null)
        {
            return TypedResults.Redirect($"/login?error={Uri.EscapeDataString("Invalid login attempt.")}");
        }

        var result = await signInManager.PasswordSignInAsync(user, request.Password, request.RememberMe, lockoutOnFailure: false);

        if (result.Succeeded)
        {
            return TypedResults.Redirect(GetSafeReturnUrl(request.ReturnUrl));
        }

        return TypedResults.Redirect($"/login?error={Uri.EscapeDataString("Invalid login attempt.")}");
    }

    private static async Task<IResult> LogoutAsync(
        [FromForm] LogoutRequest request,
        SignInManager<ApplicationUser> signInManager)
    {
        await signInManager.SignOutAsync();
        return TypedResults.Redirect(GetSafeReturnUrl(request.ReturnUrl));
    }

    private static string GetSafeReturnUrl(string? returnUrl)
    {
        return !string.IsNullOrWhiteSpace(returnUrl) && Uri.IsWellFormedUriString(returnUrl, UriKind.Relative)
            ? returnUrl
            : "/";
    }

    private sealed class LoginRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; init; } = string.Empty;

        [Required]
        public string Password { get; init; } = string.Empty;

        public bool RememberMe { get; init; }

        public string? ReturnUrl { get; init; }
    }

    private sealed class LogoutRequest
    {
        public string? ReturnUrl { get; init; }
    }
}

internal static class MiniValidator
{
    public static bool TryValidate<TModel>(TModel model)
    {
        return Validator.TryValidateObject(
            model!,
            new ValidationContext(model!),
            [],
            validateAllProperties: true);
    }
}
