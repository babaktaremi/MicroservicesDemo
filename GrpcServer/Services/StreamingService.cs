
using Grpc.Core;
using GrpcExploration;

namespace GrpcServer.Services

{
    public class StreamingService: RandomNumberFileStreaming.RandomNumberFileStreamingBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<StreamingService> _logger;
        public StreamingService(IWebHostEnvironment env, ILogger<StreamingService> logger)
        {

            _env = env;
            _logger = logger;
        }

        public override async Task StreamFile(IAsyncStreamReader<FileContent> requestStream, IServerStreamWriter<Result> responseStream, ServerCallContext context)
        {
            _logger.LogInformation("Receiving File...");

            var fileName = _env.ContentRootPath + "/Files/" + DateTime.Now.ToString("T").Replace(" ", "-").Replace(":", "-") + ".jpg";
            await using var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            int result = 0;

            try
            {
                await foreach (var file in requestStream.ReadAllAsync())
                {
                    await fs.WriteAsync(file.File.ToArray(), 0, file.File.Length);

                    await Task.Delay(200);

                    var streamResult = new Result { Percent = ++result };

                    await responseStream.WriteAsync(streamResult);
                }
            }
            finally
            {
                fs.Close();
                _logger.LogInformation("Saving File Completed");
            }
        }
    }
}
