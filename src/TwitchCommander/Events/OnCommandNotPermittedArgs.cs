using System;
using System.Collections.Generic;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;

namespace TaleLearnCode.TwitchCommander.Events
{
	public class OnCommandNotPermittedArgs : EventArgs
	{
		public List<string> ArgumentsAsList { get; }
		public string ArgumentsAsString { get; }
		public ChatMessage ChatMessage { get; }
		public string CommandText { get; }
		public ChatCommand ChatCommand { get; }

		public OnCommandNotPermittedArgs(OnChatCommandReceivedArgs onChatCommandReceivedArgs, ChatCommand chatCommand)
		{
			ArgumentsAsList = onChatCommandReceivedArgs.Command.ArgumentsAsList;
			ArgumentsAsString = onChatCommandReceivedArgs.Command.ArgumentsAsString;
			ChatMessage = onChatCommandReceivedArgs.Command.ChatMessage;
			CommandText = onChatCommandReceivedArgs.Command.CommandText;
			ChatCommand = chatCommand;
		}

	}

}