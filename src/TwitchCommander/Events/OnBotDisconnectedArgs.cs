using System;
using TwitchLib.Client.Events;

namespace TaleLearnCode.TwitchCommander.Events
{

	/// <summary>
	/// Arguments returned when <see cref="WOPR"/> disconnects from Twitch.
	/// </summary>
	/// <seealso cref="EventArgs" />
	public class OnBotDisconnectedArgs : EventArgs
	{

		public string BotUsername { get; init; }

		/// <summary>
		/// Initializes a new instance of the <see cref="OnBotDisconnectedArgs"/> class.
		/// </summary>
		/// <param name="botUsername">The username using the bot.</param>
		public OnBotDisconnectedArgs(string botUsername)
		{
			BotUsername = botUsername;
		}

	}

}