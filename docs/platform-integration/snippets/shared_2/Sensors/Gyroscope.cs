
using System;

namespace PlatformIntegration.Sensors
{
    public class GyroscopeTest
    {
        public void ToggleGyroscope()
        {
            const SensorSpeed speed = SensorSpeed.UI;

            try
            {
                if (Gyroscope.IsMonitoring)
                {
                    Gyroscope.Stop();
                    Gyroscope.ReadingChanged -= Gyroscope_ReadingChanged;
                }
                else
                {
                    Gyroscope.ReadingChanged += Gyroscope_ReadingChanged;
                    Gyroscope.Start(speed);
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

        void Gyroscope_ReadingChanged(object sender, GyroscopeChangedEventArgs e)
        {
            GyroscopeData data = e.Reading;

            // Process Angular Velocity X, Y, and Z reported in rad/s
            Console.WriteLine($"Reading: X: {data.AngularVelocity.X}, Y: {data.AngularVelocity.Y}, Z: {data.AngularVelocity.Z}");
        }
    }
}
