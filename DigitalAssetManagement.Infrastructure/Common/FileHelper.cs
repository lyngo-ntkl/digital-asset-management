using Microsoft.Extensions.Hosting;

namespace DigitalAssetManagement.Infrastructure.Common
{
    public interface FileHelper
    {
        string UploadFile(Stream fileInput, string fileName, string directoryPath);
    }

    public class FileHelperImplementation: FileHelper
    {
        private const int BufferSize = 2048;
        private const string BaseDirectory = "Files";
        private readonly IHostEnvironment _env;

        public FileHelperImplementation(IHostEnvironment env)
        {
            _env = env;
        }

        public string UploadFile(Stream fileInput, string fileName, string directoryPath)
        {
            // TODO: modify create folder & drive & user
            byte[] buffer = new byte[BufferSize];
            var filePath = $"{directoryPath}/{fileName}";
            var absolutePath = $"{_env.ContentRootPath}/{BaseDirectory}/{filePath}";
            var fileOutput = new FileStream(absolutePath, FileMode.CreateNew);
            var fileWriter = new BinaryWriter(fileOutput);
            int n = fileInput.Read(buffer, 0, BufferSize);
            while (n > 0)
            {
                fileWriter.Write(buffer, 0, n);
                n = fileInput.Read(buffer, 0, BufferSize);
            }
            fileInput.Close();
            fileOutput.Close();
            fileWriter.Close();
            return filePath;
        }
    }
}
