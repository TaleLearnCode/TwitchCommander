using System;
using TwitchLib.Client.Events;

namespace TaleLearnCode.TwitchCommander.Events
{

	/// <summary>
	/// Arguments returned when <see cref="WOPR"/> connects to a Twitch channel.
	/// </summary>
	/// <seealso cref="EventArgs" />
	public class OnBotConnectedArgs : EventArgs
	{

		/// <summary>
		/// Gets or sets the Twitch username being used by the bot.
		/// </summary>
		/// <value>
		/// A <c>string</c> representing the bot username.
		/// </value>
		public string BotUsername { get; init; }

		/// <summary>
		/// Gets or sets the Twitch channel the bot is connected to.
		/// </summary>
		/// <value>
		/// A <c>string</c> representing the name of the Twitch channel the bot is connected to.
		/// </value>
		public string AutoJoinChannel { get; init; }

		/// <summary>
		/// Initializes a new instance of the <see cref="OnBotConnectedArgs"/> class.
		/// </summary>
		/// <param name="onConnectedArgs">The event arguments from the <see cref="TwitchLib.Client.TwitchClient.OnConnected"/> event.</param>
		public OnBotConnectedArgs(OnConnectedArgs onConnectedArgs)
		{
			BotUsername = onConnectedArgs.BotUsername;
			AutoJoinChannel = onConnectedArgs.AutoJoinChannel;
		}

	}

}