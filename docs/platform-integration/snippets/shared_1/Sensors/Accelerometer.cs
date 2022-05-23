
using System;

namespace PlatformIntegration.Sensors
{
    public class AccelerometerTest
    {
        public void ToggleAccelerometer()
        {
            const SensorSpeed speed = SensorSpeed.UI;

            try
            {
                if (Accelerometer.IsMonitoring)
                {
                    Accelerometer.Stop();
                    Accelerometer.ReadingChanged -= Accelerometer_ReadingChanged;
                }
                else
                {
                    Accelerometer.ReadingChanged += Accelerometer_ReadingChanged;
                    Accelerometer.Start(speed);
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

        void Accelerometer_ReadingChanged(object sender, AccelerometerChangedEventArgs e)
        {
            AccelerometerData data = e.Reading;

            // Process Acceleration X, Y, Z
            Console.WriteLine($"Reading: X: {data.Acceleration.X}, Y: {data.Acceleration.Y}, Z: {data.Acceleration.Z}");
        }
    }
}
