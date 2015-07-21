using System;
using SunBurn.BLL;

namespace SunBurn.BLL
{
	public interface ISunburnTimerService
	{
		void StartTimer(TimeSpan time);
		void PauseTimer();
		void StopTimer();
		event SunBurn.BLL.SunburnTimerService.TimerTickEventHandler TimerTick;
	}
}

