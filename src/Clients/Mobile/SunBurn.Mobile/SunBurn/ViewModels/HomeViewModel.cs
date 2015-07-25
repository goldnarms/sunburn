using System;

namespace SunBurn
{
	public class HomeViewModel : BaseViewModel
	{
		public HomeViewModel ()
		{
			Title = "SunBurn";
			CanLoadMore = true;
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
	}
}

