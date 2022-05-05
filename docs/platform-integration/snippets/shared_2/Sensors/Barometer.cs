
using System;

namespace PlatformIntegration.Sensors
{
    public class BarometerTest
    {
        public void ToggleBarometer()
        {
            const SensorSpeed speed = SensorSpeed.UI;

            try
            {
                if (Barometer.IsMonitoring)
                {
                    Barometer.Stop();
                    Barometer.ReadingChanged -= Barometer_ReadingChanged;
                }
                else
                {
                    Barometer.ReadingChanged += Barometer_ReadingChanged;
                    Barometer.Start(speed);
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

        void Barometer_ReadingChanged(object sender, BarometerChangedEventArgs e)
        {
            var data = e.Reading;

            // Process Pressure
            Console.WriteLine($"Reading: Pressure: {data.PressureInHectopascals} hectopascals");
        }
    }
}
