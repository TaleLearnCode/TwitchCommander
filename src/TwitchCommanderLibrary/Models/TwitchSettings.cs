namespace TaleLearnCode.TwitchCommander.Models
{

	/// <summary>
	/// Represents the settings for a Twitch channel.
	/// </summary>
	public class TwitchSettings
	{

		/// <summary>
		/// Gets or sets the access token for the Twitch channel.
		/// </summary>
		/// <value>
		/// A <c>string</c> representing the Twitch access token.
		/// </value>
		public string AccessToken { get; set; }

		/// <summary>
		/// Gets or sets the Twitch client identifier.
		/// </summary>
		/// <value>
		/// A <c>string</c> representing the client identifier for the Twitch channel.
		/// </value>
		public string ClientId { get; set; }

		/// <summary>
		/// Gets or sets the name of the Twitch channel.
		/// </summary>
		/// <value>
		/// A <c>string</c> representing the Twitch channel name.
		/// </value>
		public string ChannelName { get; set; }

		/// <summary>
		/// Gets or sets the channel identifier.
		/// </summary>
		/// <value>
		/// A <c>string</c> representing the Twitch identifier for the channel.
		/// </value>
		public string ChannelId { get; set; }

		/// <summary>
		/// Gets or sets the name of the bot to work on behalf of the channel.
		/// </summary>
		/// <value>
		/// A <c>string</c> representing the bot name.
		/// </value>
		public string BotName { get; set; }

	}

}