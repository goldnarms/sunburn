using System;
using Xamarin.Forms;

[assembly:Dependency(typeof(SunBurn.Android.Services.DeviceService))]
namespace SunBurn.Android.Services
{
	public class DeviceService: SunBurn.BLL.IDeviceService{

		public void SetupSettings(){
		}
	}
}
