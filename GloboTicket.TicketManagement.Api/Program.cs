using GloboTicket.TicketManagement.Api;
using GloboTicket.TicketManagement.Application.Features.Events.Commands.CreateEvent;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("GloboTicket Is Starting");

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, services, confiugraion) =>
{
    confiugraion.ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext()
    .WriteTo.Console();
},
true);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
// Add application services
builder.Services.RegisterAppServices(builder.Configuration);
var app = builder.Build();

app.UseSerilogRequestLogging();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.ConfigureAppPipeline();
//await app.ResetDatabaseAsync();

app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();

// Map application middlewares

app.Run();
