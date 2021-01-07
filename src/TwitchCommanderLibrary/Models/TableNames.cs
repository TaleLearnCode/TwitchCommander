namespace TaleLearnCode.TwitchCommander.Models
{

	/// <summary>
	/// Represents the names of the Azure Storage tables.
	/// </summary>
	public class TableNames
	{

		/// <summary>
		/// Gets or sets the name of the table used to store the chat commands.
		/// </summary>
		/// <value>
		/// A <c>string</c> representing the name of the Chat Command table.
		/// </value>
		public string ChatCommand { get; set; }

		/// <summary>
		/// Gets or sets the name of the table used to store the chat command aliases.
		/// </summary>
		/// <value>
		/// A <c>string</c> representing the name of the Chat Command Aliases table.
		/// </value>
		public string ChatCommandAliases { get; set; }

		/// <summary>
		/// Gets or sets the name of the table used to store the chat command activity log.
		/// </summary>
		/// <value>
		/// A <c>string</c> representing the name of the Chat Command Activity table.
		/// </value>
		public string ChatCommandActivity { get; set; }

		/// <summary>
		/// Gets or sets the name of the table used to store the project tracking information.
		/// </summary>
		/// <value>
		/// A <c>string</c> representing the name of the Project Tracking table.
		/// </value>
		public string ProjectTracking { get; set; }

		/// <summary>
		/// Gets or sets the name of the table used to store the timer bot details.
		/// </summary>
		/// <value>
		/// A <c>string</c> representing the name of the Timer Bot table.
		/// </value>
		public string TimerBot { get; set; }

	}

}