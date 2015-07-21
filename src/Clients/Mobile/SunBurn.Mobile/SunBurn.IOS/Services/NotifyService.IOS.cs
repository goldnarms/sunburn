using System;
using SunBurn.BLL;
using Xamarin.Forms;


[assembly:Dependency(typeof(SunBurn.IOS.Services.NotfyService))]
namespace SunBurn.IOS.Services
{
	public class NotifyService:INotifyService{

		public void NotifyUser(string title, string message){
			var notification = new UILocalNotification ();
			notification.AlertTitle = title;
			notification.AlertAction = title;
			notification.AlertBody = message;
			notification.FireDate = NSDate.FromTimeIntervalSinceNow (10);
			UIApplication.SharedApplication.ScheduleLocalNotification (notification);
		}
	}
}
