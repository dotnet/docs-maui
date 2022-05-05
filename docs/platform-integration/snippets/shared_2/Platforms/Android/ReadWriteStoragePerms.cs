using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformIntegration.Android.Permissions
{
#if ANDROID
    public class ReadWriteStoragePerms: Microsoft.Maui.ApplicationModel.Permissions.BasePlatformPermission
    {
        public override (string androidPermission, bool isRuntime)[] RequiredPermissions =>
            new List<(string androidPermission, bool isRuntime)>
            {
                (global::Android.Manifest.Permission.ReadExternalStorage, true),
                (global::Android.Manifest.Permission.WriteExternalStorage, true)
            }.ToArray();
    }
#endif
}
