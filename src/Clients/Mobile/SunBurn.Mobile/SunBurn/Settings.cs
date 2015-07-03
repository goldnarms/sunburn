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

		public static BLL.Types.SkinType SkinTypeSetting
		{
			get
			{
				return (BLL.Types.SkinType)AppSettings.GetValueOrDefault("skin_type", (int)0);
			}
			set
			{
				//if value has changed then save it!
				AppSettings.AddOrUpdateValue("skin_type", (int)value);
			}
		}
	}
}

