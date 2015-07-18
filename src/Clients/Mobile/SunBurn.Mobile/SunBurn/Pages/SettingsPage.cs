using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace SunBurn
{
	public class SettingsPage: ContentPage
	{

		private ILocationService _locationService;
		private Picker _picker;
		private Button _btnOk;
		private ListView _locationResultList;
		private Dictionary<string, Tuple<double, double>> _locationResults;
		private Dictionary<string, SkinType> _skinTypes;
		private IDataService _dataService;
		private Entry _locationEntry;
		private BLL.UserLocation _currentLocation;
		public SettingsPage(ILocationService locationService, IDataService dataService)
		{
			Content = BuildContent ();
			Padding = new Thickness(10, Device.OnPlatform(20, 0, 0), 10, 5);
			_locationService = locationService;
			_dataService = dataService;
			Init ();
		}

		private async void Init(){
			_locationResults = new Dictionary<string, Tuple<double, double>> ();
			_currentLocation = await _locationService.GetUserLocation();
			_locationResults.Add ("Current location: " + _currentLocation.Name, new Tuple<double, double>(_currentLocation.Position.Latitude, _currentLocation.Position.Longitude));
			_locationResultList.ItemsSource = _locationResults;
		}

		private View BuildContent(){
			_skinTypes = new Dictionary<string, SkinType> {
				{ "Very light", SkinType.VeryLight },
				{ "Light", SkinType.Light},
				{ "Medium", SkinType.Medium},
				{"Tan", SkinType.Tan},
				{"Dark", SkinType.Dark},
				{"Very dark", SkinType.Black}
			};

			var label = new Label {
				Text = "Select your skintype",
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.CenterAndExpand
			};

			_picker = new Picker {
				Title = "Skin type",
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.CenterAndExpand
			};

			_picker.SelectedIndexChanged += OnIndexChanged;

			foreach (var item in _skinTypes) {
				_picker.Items.Add (item.Key);
			}

			_btnOk = new Button {
				Text = "OK",
				Font = Font.SystemFontOfSize (NamedSize.Large),
				BorderWidth = 1,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				IsEnabled = false
			};

			_btnOk.Clicked += OnButtonClicked;

			var locationLbl = new Label {
				Text = "Set your location",
				HorizontalOptions = LayoutOptions.StartAndExpand
			};

			_locationEntry = new Entry {
				Placeholder = "Search",
				HorizontalOptions = LayoutOptions.StartAndExpand
			};

			_locationEntry.TextChanged += OnTextChanged;

			_locationResultList = new ListView {
				HorizontalOptions = LayoutOptions.StartAndExpand
			};

			_locationResultList.ItemSelected += OnItemSelected;
			return new StackLayout{
				Children = {label, _picker, locationLbl, _locationEntry, _locationResultList, _btnOk},
				VerticalOptions = LayoutOptions.CenterAndExpand,
				HorizontalOptions = LayoutOptions.StartAndExpand
			};
		}

		void OnItemSelected(object sender, SelectedItemChangedEventArgs e){
			var seleced = (KeyValuePair<string, Tuple<double, double>>)e.SelectedItem;
			_locationEntry.Text = seleced.Key;
		}

		void OnTextChanged(object sender, TextChangedEventArgs e) {
			if(_locationEntry.Text.Length > 2){
				// search using entered location
				var result = _dataService.GetPositionForLocation(_locationEntry.Text).Result;

				var locations = new Dictionary<string, Tuple<double, double>> ();
				locations.Add (_currentLocation.Name, new Tuple<double, double>(_currentLocation.Position.Latitude, _currentLocation.Position.Longitude));
				foreach (var position in result.results) {
					var name = position.address_components.First (a => a.types.Contains ("administrative_area_level_2") || a.types.Contains ("administrative_area_level_1")).long_name;
					if (!locations.ContainsKey(name))
						locations.Add (name, new Tuple<double, double> ((double)position.geometry.location.lat, (double)position.geometry.location.lng));
				}
				_locationResultList.ItemsSource = locations;
			}
		}

		void OnButtonClicked(object sender, EventArgs e)
		{
			Settings.SkinTypeSetting = _skinTypes [_picker.Items [_picker.SelectedIndex]];
			var selectedLocation = (KeyValuePair<string, Tuple<double, double>>)_locationResultList.SelectedItem;
			Settings.LocationName = selectedLocation.Key;
			Settings.Position = new BLL.Position () {
				Altitude = 0,
				Latitude = _locationResults [selectedLocation.Key].Item1,
				Longitude = _locationResults [selectedLocation.Key].Item2
			};
			Navigation.PushAsync (new FrontPage (_locationService, _dataService));
		}	

		void OnIndexChanged(object sender, EventArgs e){
			if (_picker.SelectedIndex > -1)
			{
				_btnOk.IsEnabled = true;
			}
		}
	}
}