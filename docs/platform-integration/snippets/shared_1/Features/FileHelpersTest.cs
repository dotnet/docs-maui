using System.IO;
using System.Threading.Tasks;


namespace PlatformIntegration.Features
{
    class FileHelpersTest
    {
        public async Task<string> ReadTextFile(string filePath)
        {
            using Stream fileStream = await FileSystem.OpenAppPackageFileAsync(filePath);
            using StreamReader reader = new StreamReader(fileStream);

            return await reader.ReadToEndAsync();
        }

        public void GetFiles()
        {
            string cacheDir = FileSystem.CacheDirectory;
            string mainDir = FileSystem.AppDataDirectory;
        }
    }
}
