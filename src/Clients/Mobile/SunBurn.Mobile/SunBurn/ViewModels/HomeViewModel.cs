using System;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using SunBurn.BLL;

namespace SunBurn
{
	public class HomeViewModel : BaseViewModel
	{
		private IBandService _bandService;
		public HomeViewModel ()
		{
			Title = "SunBurn";
			CanLoadMore = true;

			_bandService = new BandService ();
		}

		private string _location;
		public string Location { 
			get{ return _location; } 
			set
			{ 
				_location = value;
				OnPropertyChanged("Location");
			}
		}

		private Command _connectToBand;

		public Command ConnectToBand { 
			get { return _connectToBand ?? (_connectToBand = new Command(async () => await ConnectToBandCommand()));}
		}

		private async Task ConnectToBandCommand(){
			if (IsBusy)
				return;
			IsBusy = true;
			try {
				await _bandService.SetupDevice();
			} catch (Exception ex) {
				
			} finally{
				IsBusy = false;
			}
		}
	}
}