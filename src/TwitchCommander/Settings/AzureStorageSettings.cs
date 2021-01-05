namespace TaleLearnCode.TwitchCommander.Settings
{


	/// <summary>
	/// Represents the settings for connecting to the Azure Storage account.
	/// </summary>
	public class AzureStorageSettings
	{

		/// <summary>
		/// Gets or sets the Azure Storage account URI.
		/// </summary>
		/// <value>
		/// A <c>string</c> representing the URI for connecting to the Azure Storage account.
		/// </value>
		public string Uri { get; set; }

		/// <summary>
		/// Gets or sets the name of the Azure Storage account to connect to.
		/// </summary>
		/// <value>
		/// A <c>string</c> representing the Azure Storage account name.
		/// </value>
		public string AccountName { get; set; }

		/// <summary>
		/// Gets or sets the key for connecting to the Azure Storage account.
		/// </summary>
		/// <value>
		/// A <c>string</c> representing the Azure Storage key.
		/// </value>
		public string AccountKey { get; set; }

		/// <summary>
		/// Gets or sets the name of the chat command table.
		/// </summary>
		/// <value>
		/// A <c>string</c> representing the name of the chat command table.
		/// </value>
		public string ChatCommandTableName { get; set; }

		/// <summary>
		/// Gets or sets the name of the chat command aliases table.
		/// </summary>
		/// <value>
		/// A <c>string</c> representing the name of the chat command aliases table.
		/// </value>
		public string ChatCommandAliasesTableName { get; set; }

		/// <summary>
		/// Gets or sets the name of the chat command activity table.
		/// </summary>
		/// <value>
		/// A <c>string</c> representing the name of the chat command activity table.
		/// </value>
		public string ChatCommandActivityTableName { get; set; }

		public string ProjectTrackingTableName { get; set; }

		/// <summary>
		/// Gets or sets the name of the timer bot table.
		/// </summary>
		/// <value>
		/// The name of the timer bot table.
		/// </value>
		public string TimerBotTableName { get; set; }

	}

}