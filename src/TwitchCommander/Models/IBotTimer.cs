using System;

namespace TaleLearnCode.TwitchCommander.Models
{

	/// <summary>
	/// Interface for types representing a Bot Timer.
	/// </summary>
	public interface IBotTimer
	{

		string ChannelName { get; set; }

		/// <summary>
		/// Gets or sets the minimum number of chat lines in the last five minutes to activate the timer.
		/// </summary>
		/// <value>
		/// A <c>int</c> representing the number of chat line threshold amount.
		/// </value>
		int ChatLines { get; set; }

		/// <summary>
		/// Gets or sets the name of the bot timer.
		/// </summary>
		/// <value>
		/// A <c>string</c> representing the bot timer name.
		/// </value>
		string BotTimerName { get; set; }

		/// <summary>
		/// Gets or sets the offline interval for the bot timer.
		/// </summary>
		/// <value>
		/// A <c>int</c> representing the number of minutes in between timer invocations when the channel is offline.
		/// </value>
		int OfflineInterval { get; set; }

		/// <summary>
		/// Gets or sets the online interval for the bot timer.
		/// </summary>
		/// <value>
		/// A <c>int</c> representing the number of minutes in between timer invocations when the channel is online.
		/// </value>
		int OnlineInterval { get; set; }

		/// <summary>
		/// Gets or sets the response message for the bot timer.
		/// </summary>
		/// <value>
		/// A <c>string</c> representing the bot timer response message.
		/// </value>
		/// <remarks>This value should be limited to 500 characters.</remarks>
		string ResponseMessage { get; set; }

		/// <summary>
		/// Gets or sets the date and time of the last execution.
		/// </summary>
		/// <value>
		/// A <see cref="DateTime"/> representing the date and time that the bot timer last executed.
		/// </value>
		DateTime LastExecution { get; set; }

	}

}