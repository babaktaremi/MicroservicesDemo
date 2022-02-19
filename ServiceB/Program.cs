using System.Diagnostics;
using Serilog;
using ServiceB.Logging;

Activity.DefaultIdFormat = ActivityIdFormat.W3C;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Host.UseSerilog(LoggingConfiguration.ConfigureLogger);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("/InventoryInquiry", () =>
    {

        app.Logger.LogWarning($"Receiving Request");

        var result = Inventory.DummyInventory;

        return result;
    })
.WithName("InventoryInquiry");

app.Run();

internal class Inventory
{
    public string ProductName { get; set; }
    public int ProductCount { get; set; }

    private Inventory()
    {
        
    }

    public static List<Inventory> DummyInventory = new List<Inventory>()
    {
        new Inventory() { ProductCount = 0, ProductName = "Electric Board" },
        new() { ProductCount = 10, ProductName = "LCD" }
    };
}