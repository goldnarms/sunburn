using SunBurn.BLL;
using Xamarin.Forms;

[assembly:Dependency(typeof(SunBurn.WP81.Services.DeviceService))]
namespace SunBurn.WP81.Services
{
	public class DeviceService : IDeviceService{

		public void SetupSettings(){
			if (IsolatedStorageSettings.ApplicationSettings.Contains("LocationConsent"))
			{
				// User has opted in or out of Location
				return;
			}
			else
			{
				MessageBoxResult result = 
					MessageBox.Show("This app accesses your phone's location. Is that ok?", 
						"Location",
						MessageBoxButton.OKCancel);

				if (result == MessageBoxResult.OK)
				{
					IsolatedStorageSettings.ApplicationSettings["LocationConsent"] = true;
				}else
				{
					IsolatedStorageSettings.ApplicationSettings["LocationConsent"] = false;
				}

				IsolatedStorageSettings.ApplicationSettings.Save();
			}
		}
	}
}
