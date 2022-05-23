
using System;

namespace PlatformIntegration.Sensors
{
    public class MagnetometerTest
    {
        public void ToggleMagnetometer()
        {
            const SensorSpeed speed = SensorSpeed.UI;

            try
            {
                if (Magnetometer.IsMonitoring)
                {
                    Magnetometer.Stop();
                    Magnetometer.ReadingChanged -= Magnetometer_ReadingChanged;
                }
                else
                {
                    Magnetometer.ReadingChanged += Magnetometer_ReadingChanged;
                    Magnetometer.Start(speed);
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Feature not supported on device
            }
            catch (Exception ex)
            {
                // Other error has occurred.
            }
        }

        void Magnetometer_ReadingChanged(object sender, MagnetometerChangedEventArgs e)
        {
            MagnetometerData data = e.Reading;

            // Process MagneticField X, Y, and Z
            Console.WriteLine($"Reading: X: {data.MagneticField.X}, Y: {data.MagneticField.Y}, Z: {data.MagneticField.Z}");
        }
    }
}
