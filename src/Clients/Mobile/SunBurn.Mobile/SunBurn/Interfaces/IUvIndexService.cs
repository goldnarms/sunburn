using System;

namespace SunBurn
{
    public interface IUvIndexService
    {
        double GetUvIndex(double lat, double lng, DateTime time);
    }
}
