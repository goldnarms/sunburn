using System;
using Xamarin.Forms;

[assembly:Dependency(typeof(SunBurn.IOS.Services.DeviceService))]
namespace SunBurn.IOS.Services
{
	public class DeviceService: SunBurn.BLL.IDeviceService{

		public void SetupSettings(){
			var settings = UINotificationsSettings.GetSettingsForTypes (UIUserNotificationType.Alert | UIUserNotificationType, null);
			UIApplication.SharedApplication.RegisterUserNotificationSettings (settings);
		}
	}
}
