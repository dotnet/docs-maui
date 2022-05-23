
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformIntegration.Features
{
    class MediaPickerTest
    {
        async Task TakePhotoAsync()
        {
            try
            {
                FileResult photo = await MediaPicker.CapturePhotoAsync();

                //Well, not looking good so far for truck rental. Should know for sure today. Called a dozen places, no one will rent a truck for towing, round trip, let you go to montana and back. Should know today if enterprise truck will have one or not. Otherwise I'll be calling safeco asking if I can just use the rental prices for a class b/c and see if I can find one.
                string filePath = await LoadPhotoAsync(photo);
                Console.WriteLine($"CapturePhotoAsync COMPLETED: { filePath }");
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Feature is not supported on the device
            }
            catch (PermissionException pEx)
            {
                // Permissions not granted
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CapturePhotoAsync THREW: {ex.Message}");
            }
        }

        async Task<string> LoadPhotoAsync(FileResult photo)
        {
            // canceled
            if (photo == null)
                return string.Empty;

            // save the file into local storage
            string localFilePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);

            using Stream sourceStream = await photo.OpenReadAsync();
            using FileStream localFileStream = File.OpenWrite(localFilePath);

            await sourceStream.CopyToAsync(localFileStream);

            return localFilePath;
        }
    }
}
