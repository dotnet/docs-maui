
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformIntegration.Features
{
    public class HapticTest
    {
        public void TriggerHapticFeedback()
        {
            try
            {
                // Perform click feedback
                HapticFeedback.Perform(HapticFeedbackType.Click);

                // Or use long press    
                HapticFeedback.Perform(HapticFeedbackType.LongPress);
            }
            catch (FeatureNotSupportedException ex)
            {
                // Feature not supported on device
            }
            catch (Exception ex)
            {
                // Other error has occurred.
            }
        }
    }
}
