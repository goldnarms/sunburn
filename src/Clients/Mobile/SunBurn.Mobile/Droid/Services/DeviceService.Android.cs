﻿using System;
using Xamarin.Forms;
using SunBurn;

[assembly:Dependency(typeof(SunBurn.Droid.Services.DeviceService))]
namespace SunBurn.Droid.Services
{
	public class DeviceService: IDeviceService{

		public DeviceService ()
		{
			
		}

		public void SetupSettings(){
		}
	}
}
