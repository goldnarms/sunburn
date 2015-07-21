// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace SunBurn.IOSWatchKitExtension
{
	[Register ("InterfaceController")]
	partial class InterfaceController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		WatchKit.WKInterfaceButton btnStartTimer { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		WatchKit.WKInterfaceLabel lblLocation { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		WatchKit.WKInterfaceLabel lblSpf { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		WatchKit.WKInterfaceLabel lblUvIndex { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (btnStartTimer != null) {
				btnStartTimer.Dispose ();
				btnStartTimer = null;
			}
			if (lblLocation != null) {
				lblLocation.Dispose ();
				lblLocation = null;
			}
			if (lblSpf != null) {
				lblSpf.Dispose ();
				lblSpf = null;
			}
			if (lblUvIndex != null) {
				lblUvIndex.Dispose ();
				lblUvIndex = null;
			}
		}
	}
}
