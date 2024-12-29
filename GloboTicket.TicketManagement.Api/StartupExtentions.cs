using GloboTicket.TicketManagement.Api.Middleware;
using GloboTicket.TicketManagement.Api.Services;
using GloboTicket.TicketManagement.Application;
using GloboTicket.TicketManagement.Application.Contracts;
using GloboTicket.TicketManagement.Identity;
using GloboTicket.TicketManagement.Identity.Models;
using GloboTicket.TicketManagement.Infrastructure;
using GloboTicket.TicketManagement.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using System.Security.Claims;

namespace GloboTicket.TicketManagement.Api;

public static class StartupExtentions
{
    /// <summary>
    /// Registers application, infrastructure, and persistence services.
    /// </summary>
    public static IServiceCollection RegisterAppServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddApplicationServices();  
        services.AddInfrastructureServices(configuration);
        services.AddPersistenceServices(configuration);
        services.AddIdentityServices(configuration);
        services.AddEndpointsApiExplorer();

        services.AddScoped<ILoggedInUserService, LoggedInUserService>();
        services.AddHttpContextAccessor();

        return services;
    }

    public static WebApplication ConfigureAppPipeline(this WebApplication app)
    {
        app.MapIdentityApi<ApplicationUser>();
        app.MapPost("Logout", async (ClaimsPrincipal user,
            SignInManager<ApplicationUser> signInManager) =>
        {
            await signInManager.SignOutAsync();
            return TypedResults.Ok();
        });


        if (app.Environment.IsDevelopment())
            app.MapScalarApiReference();

        app.UseCustomExceptionHandler();

        return app;
    }

    public static async Task ResetDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        
        try
        {
            var context = scope.ServiceProvider.GetService<GloboTicketDbContext>();
            if(context is not null)
            {
                await context.Database.EnsureDeletedAsync();
                await context.Database.MigrateAsync();
            }
        }
        catch(Exception ex)
        {
            //add logging here later on
        }
    }
}
