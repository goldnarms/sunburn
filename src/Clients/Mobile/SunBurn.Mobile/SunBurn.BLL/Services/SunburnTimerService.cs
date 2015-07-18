using System;
using Xamarin.Forms;

namespace SunBurn.BLL
{
	public class SunburnTimerService : ISunburnTimerService
	{
		private TimeSpan _timeToSunBurn;
		private bool _timerRunning = false;
		public SunburnTimerService ()
		{
			
		}

		public void StartTimer (TimeSpan time)
		{
			_timeToSunBurn = time;
			var tick = TimeSpan.FromSeconds (1);
			Device.StartTimer (tick, (_timerRunning) => {
				if(_timeToSunBurn.Seconds > 1){
					_timeToSunBurn = _timeToSunBurn.Subtract(tick);
				}
			});
		}

		public void PauseTimer ()
		{
			_timerRunning = false;
		}

		public void StopTimer ()
		{
			_timerRunning = false;
			_timeToSunBurn = TimeSpan.Zero;
		}
	}
}

