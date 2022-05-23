using Microsoft.Maui.Controls;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace PlatformIntegration.Features
{
    class FilePickerTest
    {
        async Task<FileResult> PickAndShow(PickOptions options)
        {
            try
            {
                var result = await FilePicker.PickAsync(options);
                if (result != null)
                {
                    if (result.FileName.EndsWith("jpg", StringComparison.OrdinalIgnoreCase) ||
                        result.FileName.EndsWith("png", StringComparison.OrdinalIgnoreCase))
                    {
                        using var stream = await result.OpenReadAsync();
                        var image = ImageSource.FromStream(() => stream);
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                // The user canceled or something went wrong
            }

            return null;
        }

        public void DoStuff()
        {
            var customFileType = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                                {
                                    { DevicePlatform.iOS, new[] { "public.my.comic.extension" } }, // or general UTType values
                                    { DevicePlatform.Android, new[] { "application/comics" } },
                                    { DevicePlatform.WinUI, new[] { ".cbr", ".cbz" } },
                                    { DevicePlatform.Tizen, new[] { "*/*" } },
                                    { DevicePlatform.macOS, new[] { "cbr", "cbz" } }, // or general UTType values
                                });

            PickOptions options = new()
            {
                PickerTitle = "Please select a comic file",
                FileTypes = customFileType,
            };
        }
    }
}
