using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf;
using Grpc.Core;
using Grpc.Net.Client;
using GrpcClient.Services.FileService;
using GrpcExploration;

namespace GrpcClient.Services.GrpcService
{
    public class GrpcClientService
    {
        private readonly IFileNumberService _fileNumberService;
        private readonly ILogger<GrpcClientService> _logger;


        public GrpcClientService(IFileNumberService fileNumberService, ILogger<GrpcClientService> logger)
        {
            _fileNumberService = fileNumberService;
            _logger = logger;
        }

        public async Task SendMessage(CancellationToken stoppingToken)
        {


            using var channel = GrpcChannel.ForAddress("https://localhost:7046");

            var client = new RandomNumberFileStreaming.RandomNumberFileStreamingClient(channel);
            using var call = client.StreamFile();


            var file = _fileNumberService.GenerateImage(300, 400).FileByteData;

            var size = file.Length / 100;
            byte[] buffer = new byte[size];
            int bytesRead;

            await using Stream source = new MemoryStream(file);

            try
            {

                var t1 = Task.Run(async () =>
                {
                    while ((bytesRead = await source.ReadAsync(buffer, 0, buffer.Length, stoppingToken)) > 0)
                    {

                        await call.RequestStream.WriteAsync(new FileContent { File = ByteString.CopyFrom(buffer) });

                        await Task.Delay(100);
                    }

                    await call.RequestStream.CompleteAsync();
                }, stoppingToken);

                var t2 = Task.Run(async () =>
                {
                    await foreach (var number in call.ResponseStream.ReadAllAsync(cancellationToken: stoppingToken))
                    {
                        _logger.LogInformation($" Progress : {number.Percent} %");
                    }
                }, stoppingToken);

                await Task.WhenAll(t1, t2);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }
            finally
            {
                await call.RequestStream.CompleteAsync();
                _logger.LogWarning("Sending Done...");
            }
        }
    }
}
