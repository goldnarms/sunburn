using System;
using Refractored.Xam.Settings;
using Refractored.Xam.Settings.Abstractions;
namespace SunBurn
{
	public static class Settings
	{
		private static ISettings AppSettings
		{
			get
			{
				return CrossSettings.Current;
			}
		}

		public static SkinType SkinTypeSetting
		{
			get
			{
				var skinType = AppSettings.GetValueOrDefault("skin_type", (int)0);
				var stEnum = (SkinType)skinType;
				return stEnum;
			}
			set
			{
				var skinType = (int)value;
				AppSettings.AddOrUpdateValue("skin_type", skinType);
			}
		}

		public static Position Position {
			get {
				var lat = AppSettings.GetValueOrDefault ("lat", (double)0);
				var lng = AppSettings.GetValueOrDefault ("lng", (double)0);
				var alt = AppSettings.GetValueOrDefault ("alt", (double)0);
				return new Position {
					Latitude = lat,
					Longitude = lng,
					Altitude = alt
				};
			}
			set {
				AppSettings.AddOrUpdateValue ("lat", value.Latitude);
				AppSettings.AddOrUpdateValue ("lng", value.Longitude);
				AppSettings.AddOrUpdateValue ("alt", value.Altitude);
			}
		}

		public static string LocationName {
			get{
				return AppSettings.GetValueOrDefault ("loc_name", "");
			}
			set{
				AppSettings.AddOrUpdateValue ("loc_name", value);
			}
		}
	}
}

