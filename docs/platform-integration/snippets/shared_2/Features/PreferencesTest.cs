using Microsoft.Maui.Controls.PlatformConfiguration;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformIntegration.Features
{
    public class PreferencesTest
    {
        public void SetExamples()
        {
            // Set a string value:
            Preferences.Set("first_name", "John");

            // Set an numerical value:
            Preferences.Set("age", 28);

            // Set a boolean value:
            Preferences.Set("has_pets", true);
        }

        public void GetExamples()
        {
            string firstName = Preferences.Get("first_name", "Unknown");
            int age = Preferences.Get("age", -1);
            bool hasPets = Preferences.Get("has_pets", false);
        }

        public void ContainsExamples()
        {
            bool hasFirstName = Preferences.ContainsKey("first_name");
        }

        public void RemoveKey()
        {
            Preferences.Remove("first_name");
        }

        public void Clear()
        {
            Preferences.Clear();
            Preferences.Clear("shared_first_name");
        }
    }
}
