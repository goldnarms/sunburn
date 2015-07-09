using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace SunBurn
{
	public class SetSkintypePage: ContentPage
	{
		private Picker _picker;
		private Button _btnOk;
		private Dictionary<string, SkinType> _skinTypes;
		public SetSkintypePage()
		{
			Content = BuildContent ();
			Padding = new Thickness(10, Device.OnPlatform(20, 0, 0), 10, 5);
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

			return new StackLayout{
				Children = {label, _picker, _btnOk}
			};
		}

		void OnButtonClicked(object sender, EventArgs e)
		{
			Settings.SkinTypeSetting = _skinTypes[_picker.Items[_picker.SelectedIndex]];			
			Navigation.PushAsync (new FrontPage ());
		}	

		void OnIndexChanged(object sender, EventArgs e){
			if (_picker.SelectedIndex > -1)
			{
				_btnOk.IsEnabled = true;
			}
		}
	}
}