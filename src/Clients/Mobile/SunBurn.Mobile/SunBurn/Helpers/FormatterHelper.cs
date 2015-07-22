using System;
using System.Linq;

namespace SunBurn
{
	public static class FormatterHelper
	{
		public static string FormatAddress(Result result){
			return FormatAddress(result.address_components);
		}

		public static string FormatAddress(Address_Components[] addressComponents){
			return string.Format ("{0}, {1}", 
				addressComponents.FirstOrDefault(a => a.types.Contains("administrative_area_level_1") || a.types.Contains("administrative_area_level_2")).long_name,
				addressComponents.FirstOrDefault(a => a.types.Contains("administrative_area_level_1") || a.types.Contains("administrative_area_level_2")).long_name
			);
		}
	}
}