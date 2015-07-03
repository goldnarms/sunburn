using System;
using System.Threading.Tasks;

namespace SunBurn
{
	public interface IDataService
	{
		Task<SunBurn.BLL.Types.Response> GetData(Tuple<double, double, double> position, double spf, BLL.Types.SkinType skinType);
	}
}

