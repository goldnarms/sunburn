using SunBurn.BLL;
using Xamarin.Forms;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

[assembly:Dependency(typeof(SunBurn.WP81.Services.PositionService))]
namespace SunBurn.WP81.Service
{
	public class PositionService: IPositionService{
		public PositionService ()
		{

		}

		public async Task<Tuple<double, double, double>> GetCurrentPosition ()
		{
			if ((bool)IsolatedStorageSettings.ApplicationSettings["LocationConsent"] != true)
			{
				// The user has opted out of Location.
				return;
			}

			Geolocator geolocator = new Geolocator();
			geolocator.DesiredAccuracyInMeters = 50;

			try
			{
				Geoposition geoposition = await geolocator.GetGeopositionAsync(
					maximumAge: TimeSpan.FromMinutes(5),
					timeout: TimeSpan.FromSeconds(10)
				);
				return new Tuple<double,double,double>(geoposition.Coordinate.Latitude, geoposition.Coordinate.Longitude, 0);
				// check for altitude
			}
			catch (Exception ex)
			{
				if ((uint)ex.HResult == 0x80004004)
				{
					// the application does not have the right capability or the location master switch is off
					StatusTextBlock.Text = "location  is disabled in phone settings.";
				}
				//else
				{
					// something else happened acquring the location
				}
			}
		}
	}
}
