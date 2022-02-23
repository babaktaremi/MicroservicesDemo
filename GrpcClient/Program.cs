using GrpcClient;
using GrpcClient.Services.FileService;
using GrpcClient.Services.GrpcService;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
        services.AddScoped<IFileNumberService, FileNumberService>();
        services.AddScoped<GrpcClientService>();
    })
    .Build();

await host.RunAsync();
