using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatformIntegration
{
    internal class PermissionsPerPlatform
    {
        public interface IReadWritePermission
        {
            Task<PermissionStatus> CheckStatusAsync();
            Task<PermissionStatus> RequestAsync();
        }

#if ANDROID
        public class ReadWriteStoragePerms : Permissions.BasePlatformPermission
        {
            public override (string androidPermission, bool isRuntime)[] RequiredPermissions =>
                new List<(string androidPermission, bool isRuntime)>
                {
                    (global::Android.Manifest.Permission.ReadExternalStorage, true),
                    (global::Android.Manifest.Permission.WriteExternalStorage, true)
                }
                .ToArray();
        }
#endif
#if WINDOWS
        public class ReadWriteStoragePerms : Permissions.BasePlatformPermission
        {

        }
#endif
#if IOS
#endif
    }
}
