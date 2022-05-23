
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformIntegration.Features
{
    public partial class DisplayInfoTest
    {
        public DisplayInfoTest()
        {
            // Subscribe to changes of screen metrics
            DeviceDisplay.MainDisplayInfoChanged += OnMainDisplayInfoChanged;
        }

        void OnMainDisplayInfoChanged(object sender, DisplayInfoChangedEventArgs e)
        {
            // Process changes
            DisplayInfo displayInfo = e.DisplayInfo;
        }
    }

    public partial class DisplayInfoTest
    {
        public void ReadInfo()
        {
            // Get Metrics
            DisplayInfo mainDisplayInfo = DeviceDisplay.MainDisplayInfo;

            // Orientation (Landscape, Portrait, Square, Unknown)
            DisplayOrientation orientation = mainDisplayInfo.Orientation;

            // Rotation (0, 90, 180, 270)
            DisplayRotation rotation = mainDisplayInfo.Rotation;

            // Width (in pixels)
            double width = mainDisplayInfo.Width;

            // Height (in pixels)
            double height = mainDisplayInfo.Height;

            // Screen density
            double density = mainDisplayInfo.Density;
        }

        public void ToggleScreenLock()
        {
            DeviceDisplay.KeepScreenOn = !DeviceDisplay.KeepScreenOn;
        }
    }
}
