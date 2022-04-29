
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformIntegration.Features
{
    public class DeviceInfoTest
    {
        public void ReadInfo()
        {
            // Device Model (SMG-950U, iPhone10,6)
            string device = DeviceInfo.Model;

            // Manufacturer (Samsung)
            string manufacturer = DeviceInfo.Manufacturer;

            // Device Name (Motz's iPhone)
            string deviceName = DeviceInfo.Name;

            // Operating System Version Number (7.0)
            string version = DeviceInfo.VersionString;

            // Platform (Android)
            DevicePlatform platform = DeviceInfo.Platform;

            // Idiom (Phone)
            DeviceIdiom idiom = DeviceInfo.Idiom;

            // Device Type (Physical)
            DeviceType deviceType = DeviceInfo.DeviceType;
        }

        public void WritePlatform()
        {
            if (DeviceInfo.Platform == DevicePlatform.Android)
                Console.WriteLine("The current OS is Android");
            else if (DeviceInfo.Platform == DevicePlatform.iOS)
                Console.WriteLine("The current OS is iOS");
            else if (DeviceInfo.Platform == DevicePlatform.UWP)
                Console.WriteLine("The current OS is Windows");
            else if (DeviceInfo.Platform == DevicePlatform.Tizen)
                Console.WriteLine("The current OS is Tizen");
        }

        public void WriteIdiom()
        {
            if (DeviceInfo.Idiom == DeviceIdiom.Desktop)
                Console.WriteLine("The current device is a desktop");
            else if (DeviceInfo.Idiom == DeviceIdiom.Phone)
                Console.WriteLine("The current device is a phone");
            else if (DeviceInfo.Idiom == DeviceIdiom.Tablet)
                Console.WriteLine("The current device is a Tablet");
        }
    }
}
