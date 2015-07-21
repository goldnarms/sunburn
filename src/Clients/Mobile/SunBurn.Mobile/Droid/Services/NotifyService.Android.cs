using System;
using SunBurn.BLL;
using Xamarin.Forms;
using Android.App;
using Android.Content;

[assembly:Dependency(typeof(SunBurn.Droid.Services.NotifyService))]
namespace SunBurn.Droid.Services
{
	public class NotifyService: INotifyService
	{
		Android.Content.Context _context;
		NotificationManager _manager;
		public NotifyService (Android.Content.Context context)
		{
			_context = context;
			_manager = context.GetSystemService (Context.NotificationService) as NotificationManager;
		}

		public void NotifyUser (string title, string message)
		{
			var _builder = new Notification.Builder (_context)
				.SetContentTitle (title)
				.SetContentText (message)
				.SetSmallIcon(Resource.Drawable.icon);
			var notification = _builder.Build ();
			var notificationId = DateTime.UtcNow.Year * 1000 + DateTime.Now.DayOfYear; // use the same notification for each day
			_manager.Notify (notificationId, notification);
		}
	}
}

