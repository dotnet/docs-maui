using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PlatformIntegration.Features
{
    public class ShareTest
    {
        public async Task ShareText(string text)
        {
            await Share.RequestAsync(new ShareTextRequest
            {
                Text = text,
                Title = "Share Text"
            });
        }

        public async Task ShareUri(string uri)
        {
            await Share.RequestAsync(new ShareTextRequest
            {
                Uri = uri,
                Title = "Share Web Link"
            });
        }

        public async Task ShareFile()
        {
            string fn = "Attachment.txt";
            string file = Path.Combine(FileSystem.CacheDirectory, fn);

            File.WriteAllText(file, "Hello World");

            await Share.RequestAsync(new ShareFileRequest
            {
                Title = "Share text file",
                File = new ShareFile(file)
            });
        }

        public async Task ShareMultipleFiles()
        {
            string file1 = Path.Combine(FileSystem.CacheDirectory, "Attachment1.txt");
            string file2 = Path.Combine(FileSystem.CacheDirectory, "Attachment2.txt");
            
            File.WriteAllText(file1, "Content 1");
            File.WriteAllText(file2, "Content 2");

            await Share.RequestAsync(new ShareMultipleFilesRequest
            {
                Title = "Share multiple files",
                Files = new List<ShareFile> { new ShareFile(file1), new ShareFile(file2) }
            });
        }
    }
}
