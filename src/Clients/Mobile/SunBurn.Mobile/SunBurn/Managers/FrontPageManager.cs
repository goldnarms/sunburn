using System;
using System.Threading.Tasks;

namespace SunBurn.Managers
{
	public class FrontPageManager
	{
		private ILocationManager _locationMngr;
		private IDataService _dataService;
		public FrontPageManager (ILocationManager locationMngr, IDataService dataService)
		{
			_locationMngr = locationMngr;
			_dataService = dataService;
		}

		public Tuple<double, double, double> GetCurrentLocation(){
			return _locationMngr.GetLocation();
		}

		public async Task<SunBurn.BLL.Types.Response> GetResult(Tuple<double, double, double> position){
			return await _dataService.GetData(position, 10, SunBurn.BLL.Types.SkinType.Light);
		}
	}
}

