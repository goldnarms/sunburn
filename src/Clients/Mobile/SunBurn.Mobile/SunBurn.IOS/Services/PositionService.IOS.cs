using System;
using CoreLocation;
using Xamarin.Forms;
using UIKit;
using SunBurn.BLL;


[assembly:Dependency(typeof(SunBurn.IOS.Services.PositionService))]
namespace SunBurn.IOS.Services
{
	public class PositionService :IPositionService{
		private CLLocationManager _iPhoneLocationManager = null;
		private CLLocation _currentLocation = null;
		public PositionService()
		{
			_iPhoneLocationManager = new CLLocationManager ();
			_iPhoneLocationManager.DesiredAccuracy = 1000;

			if (UIDevice.CurrentDevice.CheckSystemVersion (6, 0)) {
				_iPhoneLocationManager.LocationsUpdated += (object sender, CLLocationsUpdatedEventArgs e) => {
					_currentLocation = e.Locations[e.Locations.Length - 1];
				};
			} else {
				#pragma warning disable 618
				// this won't be called on iOS 6 (deprecated)
				_iPhoneLocationManager.UpdatedLocation += (object sender, CLLocationUpdatedEventArgs e) => {
					_currentLocation = e.NewLocation;
				};
				#pragma warning restore 618
			}

			if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
			{
				_iPhoneLocationManager.RequestWhenInUseAuthorization();
			}


			// start updating our location, et. al.
			if (CLLocationManager.LocationServicesEnabled)
				_iPhoneLocationManager.StartUpdatingLocation ();
 		}

		public Tuple<double, double, double> GetLocation ()
		{
			if (_currentLocation == null)
				return new Tuple<double, double, double> (48.834, 2.394, 0);
			return new Tuple<double, double, double>(_currentLocation.Coordinate.Latitude, _currentLocation.Coordinate.Longitude, _currentLocation.Altitude);
		}
	}
}

