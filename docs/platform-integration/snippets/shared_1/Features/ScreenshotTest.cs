using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;


namespace PlatformIntegration.Features
{
    class ScreenshotTest
    {
        async Task<ImageSource> CaptureScreenshot()
        {
            IScreenshotResult screenshot = await Screenshot.CaptureAsync();
            Stream stream = await screenshot.OpenReadAsync();

            return ImageSource.FromStream(() => stream);
        }
    }
}
