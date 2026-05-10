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
        group.MapPost("/register", RegisterAsync);
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
            if (user.IsAdmin && string.IsNullOrWhiteSpace(request.ReturnUrl))
                return TypedResults.Redirect("/admin");

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

    private static async Task<IResult> RegisterAsync(
        [FromForm] RegisterRequest request,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager)
    {
        if (!MiniValidator.TryValidate(request))
        {
            return TypedResults.Redirect($"/register?error={Uri.EscapeDataString("Please complete all required fields.")}");
        }

        if (!string.Equals(request.Password, request.ConfirmPassword, StringComparison.Ordinal))
        {
            return TypedResults.Redirect($"/register?error={Uri.EscapeDataString("Passwords do not match.")}");
        }

        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            IsAdmin = request.IsAdmin,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            var errorMessage = result.Errors.FirstOrDefault()?.Description ?? "Registration failed.";
            return TypedResults.Redirect($"/register?error={Uri.EscapeDataString(errorMessage)}");
        }

        await signInManager.SignInAsync(user, isPersistent: false);
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

    private sealed class RegisterRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; init; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string Password { get; init; } = string.Empty;

        [Required]
        public string ConfirmPassword { get; init; } = string.Empty;

        public bool IsAdmin { get; init; }

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
