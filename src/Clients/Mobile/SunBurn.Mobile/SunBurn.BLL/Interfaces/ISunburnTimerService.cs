using System;
using SunBurn.BLL;

namespace SunBurn
{
	public interface ISunburnTimerService
	{
		void StartTimer(TimeSpan time);
		void PauseTimer();
		void StopTimer();
		event SunBurn.SunburnTimerService.TimerTickEventHandler TimerTick;
	}
}

