using System;

using Xamarin.Forms;

namespace SunBurn
{
	public class Styles
	{
		private static Color backgroundColor = Color.FromRgb(255, 180, 5);
		private static Color baseFontColor = Color.FromRgb (252, 252, 252);
		private static Color linkColor = Color.FromRgb(15, 76, 119);
		private static Color alertColor = Color.FromRgb(24, 37, 14);
		private static Color borderColor = Color.FromRgb(4, 4, 4);
		private static string headerFont = Device.OnPlatform<string>("DamascusLight", "sans-serif-thin", "Segoe WP");
		private static string baseFont = Device.OnPlatform<string>("Thonburi", "sans-serif", "Segoe WP");

		public static Style buttonStyle = new Style (typeof(Button))
			.Set (Button.BorderColorProperty, borderColor)
			.Set (Button.HeightRequestProperty, 42)
			;

		public static Style infoLabelStyle = new Style(typeof(Label))
			.Set(Label.FontSizeProperty, Device.GetNamedSize(NamedSize.Medium, typeof(Label)))
			.Set(Label.TextColorProperty, baseFontColor)
			.Set(Label.FontFamilyProperty, baseFont)
			;

		public static Style alertLabelStyle = new Style(typeof(Label))
			.Set(Label.FontSizeProperty, Device.GetNamedSize(NamedSize.Medium, typeof(Label)))
			.Set(Label.TextColorProperty, alertColor)
			.Set(Label.FontFamilyProperty, baseFont)
			;

		public static Style headerLabelStyle = new Style(typeof(Label))
			.Set(Label.FontSizeProperty, Device.GetNamedSize(NamedSize.Large, typeof(Label)))
			.Set(Label.TextColorProperty, baseFontColor)
			.Set(Label.FontFamilyProperty, headerFont)
			;

		public static Style linkLabelStyle = new Style(typeof(Label))
			.Set(Label.FontSizeProperty, Device.GetNamedSize(NamedSize.Small, typeof(Label)))
			.Set(Label.TextColorProperty, linkColor)
			.Set(Label.FontFamilyProperty, baseFont)
			;

		public static Style backgroundLayoutStyle = new Style(typeof(Page))
			.Set(Page.BackgroundColorProperty, backgroundColor);
		
	}
}


