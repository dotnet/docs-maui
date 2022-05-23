using System;
using System.IO;
using System.Threading.Tasks;


namespace PlatformIntegration.Features
{
    class FlashlightTest
    {
        public async Task RunFlashlight()
        {
            try
            {
                // Turn On
                await Flashlight.TurnOnAsync();

                // Pause for 3 seconds
                await Task.Delay(TimeSpan.FromSeconds(3));

                // Turn Off
                await Flashlight.TurnOffAsync();
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
            }
            catch (Exception ex)
            {
                // Unable to turn on/off flashlight
            }
        }
    }
}
