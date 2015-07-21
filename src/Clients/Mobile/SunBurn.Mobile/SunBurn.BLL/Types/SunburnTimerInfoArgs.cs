using System;

namespace SunBurn.BLL
{
	public class SunburnTimerInfoArgs : EventArgs
	{
		public TimeSpan remainingTime;
		public bool isRunning;
		public SunburnTimerInfoArgs (TimeSpan remainingTime, bool isRunning) 
		{
			this.remainingTime = remainingTime;
			this.isRunning = isRunning;
		}
	}
}

