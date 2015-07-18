using System;

namespace SunBurn.BLL
{
	public interface ISunburnTimerService
	{
		void StartTimer(TimeSpan time);
		void PauseTimer();
		void StopTimer();
	}
}

