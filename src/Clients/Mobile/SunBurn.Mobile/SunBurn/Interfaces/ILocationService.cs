﻿using System;
using System.Threading.Tasks;

namespace SunBurn
{
	public interface ILocationService
	{
		Task<UserLocation> GetUserLocation();
	}
}

