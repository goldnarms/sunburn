using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunBurn.Interfaces
{
    public interface IUvIndexService
    {
        double GetUvIndex(double lat, double lng, DateTime time);
    }
}
