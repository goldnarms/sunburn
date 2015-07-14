using System;
using Xamarin.Forms;
using System.Linq;
using System.Threading.Tasks;

namespace SunBurn
{
	public class LocationService : ILocationService
	{
		IPositionService _positionService;
		IDataService _dataService;
		public LocationService (IPositionService positionService, IDataService dataService)
		{
			_positionService = positionService;
			_dataService = dataService;
		}

		public async Task<BLL.UserLocation> GetUserLocation(){
			var position = _positionService.GetCurrentPosition ();
			var locationData = await _dataService.GetLocationName(new Tuple<double, double>(position.Item1, position.Item2));
			return new BLL.UserLocation{
				Name = locationData.results.Count() > 0 ? locationData.results.First ().address_components.Where (a => a.types.Contains ("administrative_area_level_2") || a.types.Contains ("administrative_area_level_1")).First ().long_name : "",
				Position = new BLL.Position{
					Latitude = position.Item1,
					Longitude = position.Item2,
					Altitude = position.Item3
				}
			};
		}
	}
}