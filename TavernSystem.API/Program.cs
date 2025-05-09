using TavernSystem.Application;
using TavernSystem.Models;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("UniversityDatabase");
builder.Services.AddSingleton<ITavernSystemService, TavernSystemService>(tavernSystemService => new TavernSystemService(connectionString));
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/api/adventurers", (ITavernSystemService tavernSystemService) =>
{
    try
    {
        return Results.Ok(tavernSystemService.GetAllAdventurers());
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});


app.MapPost("/api/adventurers", (ITavernSystemService tavernSystemService, Adventurer anventurer) =>
{
    try
    {
        var result = tavernSystemService.AddAdventurer(anventurer);
        if (result is true)
        {
            return Results.Created("/api/adventurers", result);
        }
        else
        {
            return Results.BadRequest();
        }
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});



app.Run();

