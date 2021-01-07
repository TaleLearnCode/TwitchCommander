namespace TaleLearnCode.TwitchCommander.Models
{

	/// <summary>
	/// Represents the application settings for Twitch Commander.
	/// </summary>
	public class AppSettings
	{

		/// <summary>
		/// Gets or sets the project reminder interval.
		/// </summary>
		/// <value>
		/// A <c>int</c> representing the interval to send project reminders.
		/// </value>
		public int ProjectReminderInterval { get; set; } = 5;

		/// <summary>
		/// Gets or sets the water reminder interval.
		/// </summary>
		/// <value>
		/// A <c>int</c> representing the internval for water reminders.
		/// </value>
		public int WaterReminderInterval { get; set; } = 20;

		/// <summary>
		/// Gets or sets the stream label output path.
		/// </summary>
		/// <value>
		/// A <c>string</c> representing the ouput path for stream label files.
		/// </value>
		public string StreamLabelOutputPath { get; set; }

		/// <summary>
		/// Gets or sets the stream monitor check interval.
		/// </summary>
		/// <value>
		/// A <c>int</c> representing the interval to perform <see cref="LiveStreamMonitorService"/> checks.
		/// </value>
		public int StreamMonitorCheckInterval { get; set; }

	}

}