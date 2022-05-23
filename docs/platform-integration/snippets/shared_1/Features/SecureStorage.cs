using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PlatformIntegration.Features
{
    public class SecureStorageTest
    {
        public static async Task SetValue()
        {
            try
            {
                await SecureStorage.SetAsync("oauth_token", "secret-oauth-token-value");
            }
            catch (Exception ex)
            {
                // Possible that device doesn't support secure storage on device.
            }
        }

        public static async Task GetValue()
        {
            try
            {
                string oauthToken = await SecureStorage.GetAsync("oauth_token");

                if (oauthToken == null)
                {
                    // No value is associated with the key "oauth_token"
                }
            }
            catch (Exception ex)
            {
                // Possible that device doesn't support secure storage on device.
            }
        }

        public static void RemoveValue()
        {
            SecureStorage.Remove("oauth_token");
        }

        public static void RemoveAllValues()
        {
            SecureStorage.RemoveAll();
        }
    }
}
