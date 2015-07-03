using System;
using Android.Locations;
using Android.Content;
using Xamarin.Forms;

[assembly:Dependency(typeof(SunBurn.Droid.Services.LocationMngr))]
namespace SunBurn.Droid.Services
{
	public class LocationMngr :ILocationManager
	{
		const string Provider = LocationManager.GpsProvider;
		private readonly LocationManager _locMgr;
		private Location _currentLocation;
		public LocationMngr ()
		{
			_locMgr = Android.App.Application.Context.GetSystemService(Context.LocationService) as LocationManager;
			if(_locMgr.IsProviderEnabled(Provider)){
				var criteria = new Criteria {
					Accuracy = Accuracy.Fine,
					AltitudeRequired = true
				};
				var singleUpatePI = Android.App.PendingIntent.GetBroadcast (Android.App.Application.Context, 0, new Intent (), Android.App.PendingIntentFlags.UpdateCurrent);
				_locMgr.RequestSingleUpdate (criteria, singleUpatePI);
			}
		}

		public void OnLocationChanged(Location location){
			_currentLocation = location;
		}

		public Tuple<double, double, double> GetLocation ()
		{
			if (_currentLocation == null)
				return new Tuple<double, double, double> (0, 0, 0);
			return new Tuple<double, double, double>(_currentLocation.Latitude, _currentLocation.Longitude, _currentLocation.Altitude);
		}
	}
}

