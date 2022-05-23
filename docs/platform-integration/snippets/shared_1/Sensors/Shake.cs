
using System;

namespace PlatformIntegration.Sensors
{
    public class ShakeTest
    {
        public void ToggleAccelerometer()
        {
            const SensorSpeed speed = SensorSpeed.Game;

            try
            {
                if (Accelerometer.IsMonitoring)
                {
                    Accelerometer.Stop();
                    Accelerometer.ShakeDetected -= Accelerometer_ShakeDetected;
                }
                else
                {
                    Accelerometer.ShakeDetected += Accelerometer_ShakeDetected;
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

        void Accelerometer_ShakeDetected(object sender, EventArgs e)
        {
            // Process shake event
            Console.WriteLine("Device shake detected!");
        }
    }
}
