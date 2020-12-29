using Azure;
using Azure.Data.Tables;
using System;
using System.Linq;
using TaleLearnCode.TwitchCommander.Exceptions;
using TaleLearnCode.TwitchCommander.Extensions;
using TaleLearnCode.TwitchCommander.Helpers;
using TaleLearnCode.TwitchCommander.Models;
using TaleLearnCode.TwitchCommander.Settings;

namespace TaleLearnCode.TwitchCommander.AzureStorage
{

	public class ChatCommandActivityEntity : ITableEntity, IChatCommandActivity
	{

		/// <summary>
		/// The partition key is a unique identifier for the partition within a given table and forms the first part of an entity's primary key.
		/// </summary>
		/// <value>
		/// A string containing the partition key for the entity.
		/// </value>
		/// <remarks>The partition key is made up of the <see cref="Channel"/> and <see cref="Command"/> separated by a pipe character.</remarks>
		public string PartitionKey { get; set; }

		/// <summary>
		/// The row key is a unique identifier for an entity within a given partition. Together the <see cref="P:Azure.Data.Tables.ITableEntity.PartitionKey" /> and RowKey uniquely identify every entity within a table.
		/// </summary>
		/// <value>
		/// A string containing the row key for the entity.
		/// </value>
		/// <remarks>The row key is the Unix Time of the request in milliseconds.</remarks>
		public string RowKey { get; set; }

		/// <summary>
		/// The Timestamp property is a DateTime value that is maintained on the server side to record the time an entity was last modified.
		/// The Table service uses the Timestamp property internally to provide optimistic concurrency. The value of Timestamp is a monotonically increasing value,
		/// meaning that each time the entity is modified, the value of Timestamp increases for that entity. This property should not be set on insert or update operations (the value will be ignored).
		/// </summary>
		/// <value>
		/// A <see cref="T:System.DateTimeOffset" /> containing the timestamp of the entity.
		/// </value>
		public DateTimeOffset? Timestamp { get; set; }

		/// <summary>
		/// Gets or sets the entity's ETag. Set this value to '*' in order to force an overwrite to an entity as part of an update operation.
		/// </summary>
		/// <value>
		/// A string containing the ETag value for the entity.
		/// </value>
		public ETag ETag { get; set; }

		/// <summary>
		/// Gets or sets the channel where the command was requested.
		/// </summary>
		/// <value>
		/// A <c>string</c> representing the name of the channel where the command was requested.
		/// </value>
		public string Channel { get; set; }

		/// <summary>
		/// Gets or sets the name of the chatter requesting the command.
		/// </summary>
		/// <value>
		/// A <c>string</c> representing the name of the chatter requesting the command.
		/// </value>
		public string Chatter { get; set; }

		/// <summary>
		/// Gets or sets the command.
		/// </summary>
		/// <value>
		/// A <c>string</c> representing the name of the requested command.
		/// </value>
		public string Command { get; set; }

		/// <summary>
		/// Gets or sets the request time.
		/// </summary>
		/// <value>
		/// A <c>double</c> representing the time the chat command was requested presented as Unix time in seconds.
		/// </value>
		public double RequestTime { get; set; }

		/// <summary>
		/// Gets or sets the chat command result.
		/// </summary>
		/// <value>
		/// A <see cref="ChatCommandResult"/> representing the result of the command request.
		/// </value>
		public ChatCommandResult ChatCommandResult { get; set; }

		/// <summary>
		/// Gets or sets the request for the command.
		/// </summary>
		/// <value>
		/// A <c>string</c> representing the command typed into the chat.
		/// </value>
		public string Request { get; set; }

		/// <summary>
		/// Gets or sets the response of the command.
		/// </summary>
		/// <value>
		/// A <c>string</c> representing the response sent back to the channel's chat.
		/// </value>
		public string Response { get; set; }

		/// <summary>
		/// Saves the activity log to Azure Storage.
		/// </summary>
		/// <param name="azureStorageSettings">A <see cref="AzureStorageSettings"/> representing the settings for connecting to the Azure Storage account.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="azureStorageSettings"/> is null or contains a null <see cref="AzureStorageSettings.ChatCommandActivityTableName"/> value.</exception>
		/// <exception cref="Exception">Thrown if the <see cref="PartitionKey"/> or <see cref="RowKey"/> is not set.</exception>
		public void Save(AzureStorageSettings azureStorageSettings)
		{
			if (azureStorageSettings is null) throw new ArgumentNullException(nameof(azureStorageSettings));
			if (string.IsNullOrWhiteSpace(azureStorageSettings.ChatCommandActivityTableName)) throw new SettingMissingException(nameof(azureStorageSettings.ChatCommandActivityTableName));
			if (string.IsNullOrWhiteSpace(PartitionKey))
				throw new Exception("The PartitionKey value must be specified.");
			else if (string.IsNullOrWhiteSpace(RowKey))
				throw new Exception("The RowKey value must be specified.");
			else
				AzureStorageHelper.GetTableClient(azureStorageSettings, azureStorageSettings.ChatCommandActivityTableName).UpsertEntity(this);
		}

		/// <summary>
		/// Saves the specified <see cref="ChatCommandActivityEntity"/>.
		/// </summary>
		/// <param name="azureStorageSettings">A <see cref="AzureStorageSettings"/> representing the settings for connecting to the Azure Storage account.</param>
		/// <param name="channel">Name of the channel where the command was requested.</param>
		/// <param name="chatter">Name of the chatter requesting the command.</param>
		/// <param name="command">Name of the requested command.</param>
		/// <param name="request">The command typed into the chat</param>
		/// <param name="response">The response sent back to the channel's chat.</param>
		/// <param name="requestedDateTime">The date/time the command was requested.  The default value is Null for which the method will convert to the current date/time.</param>
		/// <param name="chatCommandResult">The result of the chat command.  The default value is <see cref="ChatCommandResult.Executed"/>.</param>
		public static void Save(
				AzureStorageSettings azureStorageSettings,
				string channel,
				string chatter,
				string command,
				string request,
				string response,
				DateTime? requestedDateTime,
				ChatCommandResult chatCommandResult = ChatCommandResult.Executed)
		{
			DateTime dateTime = (requestedDateTime != null) ? (DateTime)requestedDateTime : DateTime.UtcNow;
			ChatCommandActivityEntity chatCommandActivityEntity = new()
			{
				PartitionKey = BuildPartitionKey(channel, command),
				RowKey = dateTime.ToUnixTimeMilliseconds().ToString(),
				Channel = channel,
				Chatter = chatter,
				Command = command,
				RequestTime = dateTime.ToUnixTimeSeconds(),
				ChatCommandResult = chatCommandResult,
				Request = request,
				Response = response
			};
			chatCommandActivityEntity.Save(azureStorageSettings);
		}

		public static void Save(
			AzureStorageSettings azureStorageSettings,
				string channel,
				string chatter,
				string command,
				string request,
				string response,
				double requestedDateTime,
				ChatCommandResult chatCommandResult)
		{
			Save(azureStorageSettings, channel, chatter, command, request, response, DateTimeExtensions.UnixTimeInSecondsToDateTime(requestedDateTime), chatCommandResult);
		}

		public static void Save(
			AzureStorageSettings azureStorageSettings,
			string channel,
			string chatter,
			string command,
			string request,
			string response)
		{
			Save(azureStorageSettings, channel, chatter, command, request, response, DateTime.UtcNow, ChatCommandResult.Executed);
		}


		/// <summary>
		/// Converts the instance to a <see cref="ChatCommandActivity"/> instance.
		/// </summary>
		/// <returns>A <see cref="ChatCommandActivity"/> representing the instance.</returns>
		public ChatCommandActivity ToChatCommandActivity()
		{
			return new ChatCommandActivity()
			{
				Channel = this.Channel,
				Chatter = this.Chatter,
				Command = this.Command,
				RequestTime = this.RequestTime,
				ChatCommandResult = this.ChatCommandResult,
				Request = this.Request,
				Response = this.Response
			};
		}

		public static ChatCommandActivity GetLastCommandRequest(
			AzureStorageSettings azureStorageSettings,
			string channelName,
			string commandName,
			string chatterName = "")
		{

			if (azureStorageSettings == null) throw new ArgumentNullException(nameof(azureStorageSettings));

			ChatCommandActivityEntity response = null;
			if (!string.IsNullOrWhiteSpace(chatterName))
				response = AzureStorageHelper.GetTableClient(azureStorageSettings, azureStorageSettings.ChatCommandActivityTableName)
					.Query<ChatCommandActivityEntity>(c => c.PartitionKey == BuildPartitionKey(channelName, commandName) && c.Chatter == chatterName.ToLower())
					.OrderBy(c => c.RowKey)
					.LastOrDefault();
			else
				response = AzureStorageHelper.GetTableClient(azureStorageSettings, azureStorageSettings.ChatCommandActivityTableName)
					.Query<ChatCommandActivityEntity>(c => c.PartitionKey == BuildPartitionKey(channelName, commandName))
					.OrderBy(c => c.RowKey)
					.LastOrDefault();

			if (response != null)
				return response.ToChatCommandActivity();
			else
				return null;

		}

		public static double SecondsSinceLastRequest(AzureStorageSettings azureStorageSettings, string channelName, string commandName, string chatterName = "")
		{
			ChatCommandActivity queryResponse = GetLastCommandRequest(azureStorageSettings, channelName, commandName, chatterName);
			return (queryResponse == null) ? (DateTime.UtcNow - DateTime.UnixEpoch).TotalSeconds : DateTime.UtcNow.ToUnixTimeSeconds() - queryResponse.RequestTime;
		}

		private static string BuildPartitionKey(string channelName, string commandName)
		{
			return $"{channelName}|{commandName}".ToLower();
		}

	}

}