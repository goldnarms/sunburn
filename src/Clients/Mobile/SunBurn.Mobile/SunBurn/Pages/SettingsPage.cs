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
		private List<LocationListItem> _locationResults;
		private Dictionary<string, SkinType> _skinTypes;
		private IDataService _dataService;
		private Entry _locationEntry;
		private BLL.UserLocation _currentLocation;
		private Switch _swCelcius;

		public SettingsPage ()
		{
			var positionService = DependencyService.Get<IPositionService> ();
			_dataService = new DataService ();
			_locationService = new LocationService (positionService, _dataService);
			Init ();
		}

		public SettingsPage(ILocationService locationService, IDataService dataService)
		{
			_dataService = dataService;
			_locationService = locationService;
			Init ();
		}

		private async void Init(){
			Content = BuildContent ();
			Padding = new Thickness(10, Device.OnPlatform(20, 0, 0), 10, 5);
			Style = Styles.backgroundLayoutStyle;
			_locationResults = new List<LocationListItem> ();
			_currentLocation = await _locationService.GetUserLocation();
			_locationResults.Add (new LocationListItem { Name = "Current location: " + _currentLocation.Name, Position = new Tuple<double, double>(_currentLocation.Position.Latitude, _currentLocation.Position.Longitude)});
			_locationResultList.ItemsSource = _locationResults;
			_picker.SelectedIndex = _picker.Items.IndexOf (Enum.GetName(typeof(SkinType), Settings.SkinTypeSetting));
		}

		private View BuildContent(){
			_skinTypes = new Dictionary<string, SkinType> {
				{ "Very light", SkinType.VeryLight },
				{ "Light", SkinType.Light},
				{ "Medium", SkinType.Medium},
				{ "Tan", SkinType.Tan},
				{ "Dark", SkinType.Dark},
				{ "Very dark", SkinType.Black}
			};

			var label = new Label {
				Text = "Your skintype",
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
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				IsEnabled = false,
				Style = Styles.buttonStyle
			};

			_btnOk.Clicked += OnButtonClicked;

			var locationLbl = new Label {
				Text = "Your location",
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.CenterAndExpand
			};

			_locationEntry = new Entry {
				Placeholder = "Search",
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.CenterAndExpand
			};

			_locationEntry.TextChanged += OnTextChanged;

			_locationResultList = new ListView {
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				Header = "Location",
				ItemTemplate = new DataTemplate (typeof(TextCell)) {
					Bindings = {
						{ TextCell.TextProperty, new Binding ("Name") }
					}
				}
			};

			var celciusLbl = new Label {
				Text = "Use celcius: ",
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.CenterAndExpand,

			};

			_swCelcius = new Switch {
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.CenterAndExpand,
			};

			_swCelcius.IsToggled = Settings.PreferCelcius;

			_locationResultList.ItemSelected += OnItemSelected;
			return new StackLayout{
				Children = {label, _picker, locationLbl, _locationEntry, _locationResultList, _btnOk},
				VerticalOptions = LayoutOptions.CenterAndExpand,
				HorizontalOptions = LayoutOptions.StartAndExpand
			};
		}

		void Metric_Toggled(object sender, ToggledEventArgs args){
			Settings.PreferCelcius = args.Value;
		}

		void OnItemSelected(object sender, SelectedItemChangedEventArgs e){
			var selected = (LocationListItem)e.SelectedItem;
			_locationEntry.Text = selected.Name;
		}

		async void OnTextChanged(object sender, TextChangedEventArgs e) {
			if(_locationEntry.Text.Length > 2){
				// search using entered location
				var result = await _dataService.GetPositionForLocation(_locationEntry.Text);

				var locations = new Dictionary<string, Tuple<double, double>> ();
				locations.Add (_currentLocation.Name, new Tuple<double, double>(_currentLocation.Position.Latitude, _currentLocation.Position.Longitude));
				foreach (var position in result.results) {
					var name = FormatterHelper.FormatAddress (position);
					if (!locations.ContainsKey(name))
						locations.Add (name, new Tuple<double, double> ((double)position.geometry.location.lat, (double)position.geometry.location.lng));
				}
				_locationResultList.ItemsSource = locations;
			}
		}

		void OnButtonClicked(object sender, EventArgs e)
		{
			Settings.SkinTypeSetting = _skinTypes [_picker.Items [_picker.SelectedIndex]];
			var selectedLocation = (LocationListItem)_locationResultList.SelectedItem;
			Settings.LocationName = selectedLocation.Name;
			Settings.Position = new BLL.Position () {
				Altitude = 0,
				Latitude = selectedLocation.Position.Item1,
				Longitude = selectedLocation.Position.Item2
			};
			Navigation.PushAsync (new HomePage (_locationService, _dataService));
		}	

		void OnIndexChanged(object sender, EventArgs e){
			if (_picker.SelectedIndex > -1)
			{
				_btnOk.IsEnabled = true;
			}
		}
	}
}