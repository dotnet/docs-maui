using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PlatformIntegration.Features
{
    internal class WebAuthTest
    {
        public async Task AuthIt()
        {
            try
            {
                WebAuthenticatorResult authResult = await WebAuthenticator.AuthenticateAsync(
                    new Uri("https://mysite.com/mobileauth/Microsoft"),
                    new Uri("myapp://"));

                string accessToken = authResult?.AccessToken;

                // Do something with the token
            }
            catch (TaskCanceledException e)
            {
                // Use stopped auth
            }
        }

        public async Task AuthItSecure()
        {
            try
            {
                WebAuthenticatorResult authResult = await WebAuthenticator.AuthenticateAsync(
                    new WebAuthenticatorOptions()
                    {
                        Url = new Uri("https://mysite.com/mobileauth/Microsoft"),
                        CallbackUrl = new Uri("myapp://"),
                        PrefersEphemeralWebBrowserSession = true
                    });

                string accessToken = authResult?.AccessToken;

                // Do something with the token
            }
            catch (TaskCanceledException e)
            {
                // Use stopped auth
            }
        }

        public async Task AuthItApple()
        {
            var scheme = "..."; // Apple, Microsoft, Google, Facebook, etc.
            var authUrlRoot = "https://mysite.com/mobileauth/";
            WebAuthenticatorResult result = null;

            if (scheme.Equals("Apple")
                && DeviceInfo.Platform == DevicePlatform.iOS
                && DeviceInfo.Version.Major >= 13)
            {
                // Use Native Apple Sign In API's
                result = await AppleSignInAuthenticator.AuthenticateAsync();
            }
            else
            {
                // Web Authentication flow
                var authUrl = new Uri($"{authUrlRoot}{scheme}");
                var callbackUrl = new Uri("myapp://");

                result = await WebAuthenticator.AuthenticateAsync(authUrl, callbackUrl);
            }

            var authToken = string.Empty;

            if (result.Properties.TryGetValue("name", out string name) && !string.IsNullOrEmpty(name))
                authToken += $"Name: {name}{Environment.NewLine}";

            if (result.Properties.TryGetValue("email", out string email) && !string.IsNullOrEmpty(email))
                authToken += $"Email: {email}{Environment.NewLine}";

            // Note that Apple Sign In has an IdToken and not an AccessToken
            authToken += result?.AccessToken ?? result?.IdToken;
        }
    }
}
