var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.MapGet("/UserStatus", async (IHttpClientFactory clientFactory) =>
    {
        var client = clientFactory.CreateClient();

        var result =
            await client.GetFromJsonAsync<List<UserStatus>>("http://localhost:8574/api/v1/Users/RegistrationStatus");

        return result;
    })
.WithName("GetWeatherForecast");

app.Run();


internal record UserStatus(string UserName,DateTime RegistrationDate,bool IsRegistrationComplete);