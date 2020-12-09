using System;
using TwitchLib.Client.Events;

namespace TaleLearnCode.TwitchCommander.Events
{
	public class OnLoggedEventArgs
	{

		/// <summary>
		/// Gets the date and time of the logged event.
		/// </summary>
		/// <value>
		/// A <see cref="DateTime"/> representing the date and time the logged event occurred.
		/// </value>
		public DateTime DateTime { get; init; }

		/// <summary>
		/// Gets or sets the username of the connected bot.
		/// </summary>
		/// <value>
		/// A <c>string</c> representing the connected bot username.
		/// </value>
		public string BotUsername { get; init; }

		/// <summary>
		/// Gets or sets the data of the logged event.
		/// </summary>
		/// <value>
		/// A <c>string</c> representing the logged event data.
		/// </value>
		public string Data { get; init; }

		/// <summary>
		/// Initializes a new instance of the <see cref="OnLoggedEventArgs"/> class.
		/// </summary>
		/// <param name="onLogArgs">The <see cref="OnLogArgs"/> from the <see cref="TwitchLib.Client.TwitchClient.OnLog"/>event.</param>
		public OnLoggedEventArgs(OnLogArgs onLogArgs)
		{
			DateTime = onLogArgs.DateTime;
			BotUsername = onLogArgs.BotUsername;
			Data = onLogArgs.Data;
		}

	}

}