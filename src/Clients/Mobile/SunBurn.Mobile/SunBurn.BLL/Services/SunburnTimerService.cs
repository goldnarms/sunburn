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
			Device.StartTimer (tick, () => {

				if(_timerRunning && _timeToSunBurn.Seconds > 1){
					_timeToSunBurn = _timeToSunBurn.Subtract(tick);
					TimerTickEventHandler;
					return true;
				}
				return false;
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

		public delegate void TimerTickEventHandler();
	}
}

