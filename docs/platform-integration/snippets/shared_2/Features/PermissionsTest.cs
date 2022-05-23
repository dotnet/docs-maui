using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.ApplicationModel;


namespace PlatformIntegration.Features
{
    class PermissionsTest
    {
        public async Task SmallCheckStatus()
        {
            //<SmallCheckStatus>
            PermissionStatus status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            //</SmallCheckStatus>
        }

        public async Task SmallRequestStatus()
        {
            //<SmallRequest>
            PermissionStatus status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            //</SmallRequest>
        }

        public async Task SmallRequestStatusCustom()
        {
#if ANDROID
            //<SmallRequestCustom>
            PermissionStatus status = await Permissions.RequestAsync<PlatformIntegration.Android.Permissions.ReadWriteStoragePerms>();
            //</SmallRequestCustom>
#endif
        }

        //<CheckAndRequest>
        public async Task<PermissionStatus> CheckAndRequestLocationPermission()
        {
            PermissionStatus status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

            if (status == PermissionStatus.Granted)
                return status;

            if (status == PermissionStatus.Denied && DeviceInfo.Platform == DevicePlatform.iOS)
            {
                // Prompt the user to turn on in settings
                // On iOS once a permission has been denied it may not be requested again from the application
                return status;
            }

            if (Permissions.ShouldShowRationale<Permissions.LocationWhenInUse>())
            {
                // Prompt the user with additional information as to why the permission is needed
            }

            status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

            return status;
        }
        //</CheckAndRequest>

        //<PermissionInstance>
#nullable enable
        public async Task<Location?> GetLocationAsync()
        {
            PermissionStatus status = await CheckAndRequestPermissionAsync(new Permissions.LocationWhenInUse());
            if (status != PermissionStatus.Granted)
            {
                // Notify user permission was denied
                return null;
            }

            return await Geolocation.GetLocationAsync();
        }

        public async Task<PermissionStatus> CheckAndRequestPermissionAsync<T>(T permission)
                    where T : Permissions.BasePermission
        {
            PermissionStatus status = await permission.CheckStatusAsync();

            if (status != PermissionStatus.Granted)
                status = await permission.RequestAsync();

            return status;
        }
#nullable restore
        //</PermissionInstance>

        public class MyPermission : Permissions.BasePermission
        {
            // This method checks if current status of the permission.
            public override Task<PermissionStatus> CheckStatusAsync()
            {
                throw new System.NotImplementedException();
            }

            // This method is optional and a PermissionException is often thrown if a permission is not declared.
            public override void EnsureDeclared()
            {
                throw new System.NotImplementedException();
            }

            // Requests the user to accept or deny a permission.
            public override Task<PermissionStatus> RequestAsync()
            {
                throw new System.NotImplementedException();
            }

            // Indicates that the requestor should prompt the user as to why the app requires the permission, because the
            // user has previously denied this permission.
            public override bool ShouldShowRationale()
            {
                throw new NotImplementedException();
            }
        }

    }
}
