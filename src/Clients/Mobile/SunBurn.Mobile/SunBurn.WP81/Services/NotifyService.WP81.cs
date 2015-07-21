using SunBurn.BLL;
using Xamarin.Forms;

[assembly:Dependency(typeof(SunBurn.WP81.Services.NotifyService))]
namespace SunBurn.WP81.Services
{
	public class NotifyService: INotifyService{
		public NotifyService ()
		{
			
		}

		public void NotifyUser(string title, string message){
			ShellToast notification = new ShellToast ();
			notification.Title = title;
			notification.Content = message;
			toast.Show ();
		}
	}
}
