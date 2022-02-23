using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:9191,password=123456";
    options.InstanceName = "";
});

builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("localhost:9191,password=123456"));
builder.Services.AddSingleton<IServer>(ConnectionMultiplexer.Connect("localhost:9191,password=123456").GetServer("localhost", 9191));



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.MapPost("/Set", async (IDistributedCache cache,IServer redis) =>
    {
        if(redis.Keys().Any(c=>c.ToString().Equals("Users")))
            return;

        var users = new List<User>()
            { new("Babak", "1234"), new("Ali", "1234"), new("Hossein", "123456") };

        var content= Encoding.UTF8.GetBytes(JsonSerializer.Serialize(users));

        await cache.SetAsync("Users", content);

    })
.WithName("SetCache");

app.MapGet("/Get", async (IDistributedCache cache) =>
    {
        var cacheContent = await cache.GetStringAsync("Users");

        var users = JsonSerializer.Deserialize<List<User>>(cacheContent);
        return users;

    })
    .WithName("GetCache");


app.Run();


internal record User(string Name,string Password);