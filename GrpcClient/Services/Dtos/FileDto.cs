using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrpcClient.Services.Dtos
{
    public class FileDto
    {
        public string Number { get; set; }
        public byte[] FileByteData { get; set; }
    }
}
