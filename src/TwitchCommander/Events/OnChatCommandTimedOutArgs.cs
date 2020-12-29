using System;
using TaleLearnCode.TwitchCommander.Models;

namespace TaleLearnCode.TwitchCommander.Events
{
	public class OnChatCommandTimedOutArgs : EventArgs
	{

		public ChatCommand ChatCommand { get; set; }

		public ChatCommandActivity LastCommandActivity { get; set; }

		public ChatCommandTimeoutType CommandTimeoutType { get; set; }

		public OnChatCommandTimedOutArgs(ChatCommand chatCommand, ChatCommandActivity lastCommandActivity, ChatCommandTimeoutType commandTimeoutType)
		{
			ChatCommand = chatCommand;
			LastCommandActivity = lastCommandActivity;
			CommandTimeoutType = commandTimeoutType;
		}

	}
}