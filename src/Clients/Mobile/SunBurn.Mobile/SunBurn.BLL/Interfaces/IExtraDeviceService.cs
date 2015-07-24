using System;
using System.Threading.Tasks;

namespace SunBurn.BLL
{
	public interface IExtraDeviceService
	{
		Task Notify(string title, string message);
	}
}

