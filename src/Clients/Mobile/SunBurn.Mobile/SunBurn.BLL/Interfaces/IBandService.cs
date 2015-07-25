using System;
using System.Threading.Tasks;

namespace SunBurn.BLL
{
	public interface IBandService : IExtraDeviceService
	{
		Task SetupDevice ();
		Task UpdateTile(SunburnResult result);
	}
}

