using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.Wearable.Views;
using Android.Views;
using Android.Widget;
using SunBurn.BLL;
using Android.Hardware;
using System.Linq;

namespace SunBurn.Wear
{
	[Activity (Label = "SunBurn", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{

		SunburnTimerService _timerService;
		TimeSpan _timeToSunburn;
		TextView _lblSpfApplied;
		TextView _timeToSunburnLbl;
		TextView _locationLbl;
		IExposureCalculator _exposureCalculator;
		double _uvIndex;

		int _spfAppliedIndex = 0;
		int[] _spfFactors = {0, 15, 30, 50};

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			var v = FindViewById<WatchViewStub> (Resource.Id.watch_view_stub);
			v.LayoutInflated += delegate {
				BindComponents();
				Init ();
			};
		}

		void OnStartTimerClicked(object sender, EventArgs e)
		{
			_timerService.StartTimer(_timeToSunburn);
			var notification = new NotificationCompat.Builder (this)
				.SetContentTitle ("Button tapped")
				.SetContentText ("Button tapped times!")
				.SetSmallIcon (Android.Resource.Drawable.StatNotifyVoicemail)
				.SetGroup ("group_key_demo").Build ();

			var manager = NotificationManagerCompat.From (this);
			manager.Notify (1, notification);
		}

		void OnSpfDecreasedClicked(object sender, EventArgs e){
			if(_spfAppliedIndex > 0){
				_spfAppliedIndex--;
				_lblSpfApplied.Text = _spfFactors [_spfAppliedIndex].ToString();
				_timeToSunburn = _exposureCalculator.CalculateTimeToSunburn (SkinType.Light, _uvIndex, _spfFactors[_spfAppliedIndex], 0,false);
				_timeToSunburnLbl.Text = _timeToSunburn.ToString ();
			}
		}

		void OnSpfIncreasedClicked(object sender, EventArgs e){
			if(_spfAppliedIndex < _spfFactors.Length - 1){
				_spfAppliedIndex++;
				_lblSpfApplied.Text = _spfFactors [_spfAppliedIndex].ToString();
				_timeToSunburn = _exposureCalculator.CalculateTimeToSunburn (SkinType.Light, _uvIndex, _spfFactors[_spfAppliedIndex], 0,false);
				_timeToSunburnLbl.Text = _timeToSunburn.ToString ();
			}
		}

		private void BindComponents(){
			Button btnStartTimer = FindViewById<Button> (Resource.Id.btnStartTime);
			btnStartTimer.Click += OnStartTimerClicked;
			Button btnDecreaseSpf = FindViewById<Button>(Resource.Id.btnDecreaseSpf);
			btnDecreaseSpf.Click += OnSpfDecreasedClicked;
			Button btnIncreaseSpf = FindViewById<Button>(Resource.Id.btnIncreaseSpf);
			btnIncreaseSpf.Click += OnSpfIncreasedClicked;

			_lblSpfApplied = FindViewById<TextView> (Resource.Id.lblAppliedSpf);
			_lblSpfApplied.Text = "0";

			_locationLbl = FindViewById<TextView> (Resource.Id.lblLocation);
			_timeToSunburnLbl = FindViewById<TextView> (Resource.Id.lblTimeToSunburn);
		}

		private async void Init(){
			var uvIndexService = new UvIndexService ();
			var dataService = new DataService ();
			var positionService = new PositionService ();
			var locationService = new LocationService (positionService, dataService);
			var location = await locationService.GetUserLocation ();
			var locationResponse = await dataService.GetLocationName (new Tuple<double, double> (location.Position.Latitude, location.Position.Longitude));
			var locationName = locationResponse.results.First().address_components.First (a => a.types.Contains ("administrative_area_level_2") || a.types.Contains ("administrative_area_level_1")).long_name;

			_locationLbl.Text = locationName;
			_uvIndex = uvIndexService.GetUvIndex (location.Position.Latitude, location.Position.Longitude, DateTime.Now);
			TextView uvIndexLbl = FindViewById<TextView> (Resource.Id.lblUvIndex);
			uvIndexLbl.Text = _uvIndex.ToString();
			_exposureCalculator = new Calculators.ExposureCalculator ();
			_timeToSunburn = _exposureCalculator.CalculateTimeToSunburn (SkinType.Light, _uvIndex, _spfFactors[_spfAppliedIndex], 0,false);
			_timeToSunburnLbl.Text = _timeToSunburn.ToString ();

			_timerService = new SunburnTimerService();
			_timerService.TimerTick += new SunburnTimerService.TimerTickHandler (TimerTick);
		}

		public void TimerTick(object timer, SunburnTimerInfoArgs infoArgs)
		{
			_timeToSunburnLbl.Text = infoArgs.remainingTime.ToString ("c");
		}
	}
}



