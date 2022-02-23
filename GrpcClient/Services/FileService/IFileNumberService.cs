using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrpcClient.Services.Dtos;

namespace GrpcClient.Services.FileService
{
    public interface IFileNumberService
    {
        FileDto GenerateImage(int width, int height);
    }
}
