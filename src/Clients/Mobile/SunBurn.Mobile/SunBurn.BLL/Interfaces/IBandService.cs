using System;
using System.Threading.Tasks;

namespace SunBurn.BLL
{
	public interface IBandService : IExtraDeviceService
	{
		Task UpdateTile(SunburnResult result);
	}
}

