using Microsoft.Maui.Controls.PlatformConfiguration;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformIntegration.Features
{
    public class ClipboardTest
    {
        public ClipboardTest()
        {
            // Register for clipboard changes, be sure to unsubscribe when needed
            Clipboard.ClipboardContentChanged += OnClipboardContentChanged;
        }

        void OnClipboardContentChanged(object sender, EventArgs e)
        {
            Console.WriteLine($"Last clipboard change at {DateTime.UtcNow:T}");
        }

        public async void SetText()
        {
            await Clipboard.SetTextAsync("Hello World");
        }

        public async void GetText()
        {
            string text = await Clipboard.GetTextAsync();
        }
    }
}
