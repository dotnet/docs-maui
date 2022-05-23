
using System;

namespace PlatformIntegration.Sensors
{
    public class OrientationTest
    {
        public void ToggleOrientation()
        {
            const SensorSpeed speed = SensorSpeed.UI;

            try
            {
                if (OrientationSensor.IsMonitoring)
                {
                    OrientationSensor.Stop();
                    OrientationSensor.ReadingChanged -= OrientationSensor_ReadingChanged;
                }
                else
                {
                    OrientationSensor.ReadingChanged += OrientationSensor_ReadingChanged;
                    OrientationSensor.Start(speed);
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

        void OrientationSensor_ReadingChanged(object sender, OrientationSensorChangedEventArgs e)
        {
            OrientationSensorData data = e.Reading;

            // Process Orientation quaternion (X, Y, Z, and W)
            Console.WriteLine($"Reading: X: {data.Orientation.X}, Y: {data.Orientation.Y}, Z: {data.Orientation.Z}");
        }
    }
}
