
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformIntegration.Features
{
    public class LauncherTest
    {
        public async Task OpenRideShareAsync()
        {
            var supportsUri = await Launcher.CanOpenAsync("lyft://");

            if (supportsUri)
                await Launcher.OpenAsync("lyft://ridetype?id=lyft_line");
        }

        public async Task<bool> TryOpenRideShareAsync() =>
            await Launcher.TryOpenAsync("lyft://ridetype?id=lyft_line");

        public async Task OpenTextFile()
        {
            string popoverTitle = "Read text file";
            string name = "File.txt";
            string file = System.IO.Path.Combine(FileSystem.CacheDirectory, name);

            System.IO.File.WriteAllText(file, "Hello World");

            await Launcher.OpenAsync(new OpenFileRequest(popoverTitle, new ReadOnlyFile(file)));
        }
    }
}
