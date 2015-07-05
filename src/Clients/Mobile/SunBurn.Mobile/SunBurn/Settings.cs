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
				return (SkinType)AppSettings.GetValueOrDefault("skin_type", (int)0);
			}
			set
			{
				AppSettings.AddOrUpdateValue("skin_type", (int)value);
			}
		}
	}
}

