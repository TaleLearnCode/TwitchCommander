using System;

namespace TaleLearnCode.TwitchCommander.Models
{

	/// <summary>
	/// Represents a bot timer.
	/// </summary>
	public class BotTimer
	{

		/// <summary>
		/// Gets or sets the name of the channel.
		/// </summary>
		/// <value>
		/// The name of the channel.
		/// </value>
		public string ChannelName { get; set; }

		/// <summary>
		/// Gets or sets the name of the bot timer.
		/// </summary>
		/// <value>
		/// A <c>string</c> representing the bot timer name.
		/// </value>
		public string BotTimerName { get; set; }

		/// <summary>
		/// Gets or sets the response message for the bot timer.
		/// </summary>
		/// <value>
		/// A <c>string</c> representing the bot timer response message.
		/// </value>
		/// <remarks>
		/// This value should be limited to 500 characters.
		/// </remarks>
		public string ResponseMessage { get; set; }

		/// <summary>
		/// Gets or sets the online interval for the bot timer.
		/// </summary>
		/// <value>
		/// A <c>int</c> representing the number of minutes in between timer invocations when the channel is online.
		/// </value>
		/// <remarks>A value of zero means that the bot timer will not be triggered when online.</remarks>
		public int OnlineInterval { get; set; }

		/// <summary>
		/// Gets or sets the offline interval for the bot timer.
		/// </summary>
		/// <value>
		/// A <c>int</c> representing the number of minutes in between timer invocations when the channel is offline.
		/// </value>
		/// <remarks>A value of zero means that the bot timer will not be triggered when offline.</remarks>
		public int OfflineInterval { get; set; }

		/// <summary>
		/// Gets or sets the minimum number of chat lines in the last five minutes to activate the timer.
		/// </summary>
		/// <value>
		/// A <c>int</c> representing the number of chat line threshold amount.
		/// </value>
		public int ChatLines { get; set; }

		/// <summary>
		/// Gets or sets the date and time of the last execution.
		/// </summary>
		/// <value>
		/// A <see cref="DateTime" /> representing the date and time that the bot timer last executed.
		/// </value>
		public DateTime LastExecution { get; set; }

		/// <summary>
		/// Gets the date and time of the next execution of the time while the broadcaster is online.
		/// </summary>
		/// <value>
		/// A <see cref="DateTime"/> representing the next online execution of the bot timer.
		/// </value>
		public DateTime NextOnlineExecution
		{
			get
			{
				return (OnlineInterval > 0) ? LastExecution.AddMinutes(OnlineInterval) : DateTime.MaxValue;
			}
		}

		public DateTime NextOfflineExecution
		{
			get
			{
				return (OfflineInterval > 0) ? LastExecution.AddMinutes(OfflineInterval) : DateTime.MaxValue;
			}
		}



	}

}