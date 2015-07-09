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
	}
}

