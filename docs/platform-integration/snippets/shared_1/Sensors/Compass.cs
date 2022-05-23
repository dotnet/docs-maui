
using System;

namespace PlatformIntegration.Sensors
{
    public class CompassTest
    {
        public void ToggleCompass(ICompass compassInstance)
        {
            const SensorSpeed speed = SensorSpeed.UI;

            try
            {
                if (compassInstance.IsMonitoring)
                {
                    compassInstance.Stop();
                    compassInstance.ReadingChanged -= Compass_ReadingChanged;
                }
                else
                {
                    compassInstance.ReadingChanged += Compass_ReadingChanged;
                    compassInstance.Start(speed);
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Feature not supported on device
            }
            catch (Exception ex)
            {
                // Some other exception has occurred
            }
        }

        void Compass_ReadingChanged(object sender, CompassChangedEventArgs e)
        {
            var data = e.Reading;

            // Process Heading Magnetic North
            Console.WriteLine($"Reading: {data.HeadingMagneticNorth} degrees");
        }
    }
}
