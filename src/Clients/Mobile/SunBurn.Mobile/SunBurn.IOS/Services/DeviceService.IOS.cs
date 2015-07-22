using System;
using Xamarin.Forms;
using SunBurn;

[assembly:Dependency(typeof(SunBurn.IOS.Services.DeviceService))]
namespace SunBurn.IOS.Services
{
	public class DeviceService: IDeviceService{

		public DeviceService ()
		{

		}

		public void SetupSettings(){
			UIUserNotificationType notificationTypes = UIUserNotificationType.Alert | 
				UIUserNotificationType.Badge | 
				UIUserNotificationType.Sound;

			var userNoticationSettings = UIUserNotificationSettings.GetSettingsForTypes(notificationTypes, new NSSet(new string[] {}));

			UIApplication.SharedApplication.RegisterUserNotificationSettings (userNoticationSettings);
		}
	}
}
