using System;

namespace TaleLearnCode.TwitchCommander.Events
{

	public class OnStreamMetricsUpdatedArgs : EventArgs
	{

		public int Subscribers { get; set; }

		public int Followers { get; set; }

	}

}