using System;
using TaleLearnCode.TwitchCommander.Extensions;
using TwitchLib.Client.Models;

namespace TaleLearnCode.TwitchCommander.Models
{

	public class ReceivedChatMessage
	{

		public double Timestamp { get; set; }

		public ChatMessage ChatMessage { get; set; }

		public ReceivedChatMessage(ChatMessage chatMessage)
		{
			ChatMessage = chatMessage;
			Timestamp = DateTime.UtcNow.ToUnixTimeSeconds();
		}

	}

}