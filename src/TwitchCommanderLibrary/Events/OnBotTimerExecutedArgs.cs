using System;

namespace TaleLearnCode.TwitchCommander.Events
{

	public class OnBotTimerExecutedArgs : EventArgs
	{

		public string ChannelName { get; set; }

		public string BotTimerName { get; set; }

		public string ResponseMessage { get; set; }

		public bool ChatThresholdMet { get; set; }


	}

}