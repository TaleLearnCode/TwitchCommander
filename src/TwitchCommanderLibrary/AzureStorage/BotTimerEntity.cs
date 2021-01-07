using Azure;
using Azure.Data.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleLearnCode.TwitchCommander.Exceptions;
using TaleLearnCode.TwitchCommander.Models;

namespace TaleLearnCode.TwitchCommander.AzureStorage
{
	public class BotTimerEntity : ITableEntity
	{

		/// <summary>
		/// The partition key is a unique identifier for the partition within a given table and forms the first part of an entity's primary key.
		/// </summary>
		/// <value>
		/// A string containing the partition key for the entity.
		/// </value>
		public string PartitionKey { get; set; }

		/// <summary>
		/// The row key is a unique identifier for an entity within a given partition. Together the <see cref="P:Azure.Data.Tables.ITableEntity.PartitionKey" /> and RowKey uniquely identify every entity within a table.
		/// </summary>
		/// <value>
		/// A string containing the row key for the entity.
		/// </value>
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
		/// Gets or sets the minimum number of chat lines in the last five minutes to activate the timer.
		/// </summary>
		/// <value>
		/// A <c>int</c> representing the number of chat line threshold amount.
		/// </value>
		public int ChatLines { get; set; }

		public string ChannelName { get; set; }

		/// <summary>
		/// Gets or sets the name of the bot timer.
		/// </summary>
		/// <value>
		/// A <c>string</c> representing the bot timer name.
		/// </value>
		public string BotTimerName { get; set; }

		/// <summary>
		/// Gets or sets the offline interval for the bot timer.
		/// </summary>
		/// <value>
		/// A <c>int</c> representing the number of minutes in between timer invocations when the channel is offline.
		/// </value>
		public int OfflineInterval { get; set; }

		/// <summary>
		/// Gets or sets the online interval for the bot timer.
		/// </summary>
		/// <value>
		/// A <c>int</c> representing the number of minutes in between timer invocations when the channel is online.
		/// </value>
		public int OnlineInterval { get; set; }

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
		/// Gets or sets the date and time of the last execution.
		/// </summary>
		/// <value>
		/// A <see cref="DateTime" /> representing the date and time that the bot timer last executed.
		/// </value>
		public DateTime LastExecution { get; set; }

		public void Save(AzureStorageSettings azureStorageSettings, TableNames tableNames)
		{
			if (azureStorageSettings == null) throw new ArgumentNullException(nameof(azureStorageSettings));
			if (tableNames == null) throw new ArgumentNullException(nameof(tableNames));
			if (string.IsNullOrWhiteSpace(tableNames.TimerBot)) throw new SettingMissingException(nameof(tableNames.TimerBot));
			if (string.IsNullOrWhiteSpace(PartitionKey)) throw new Exception("The PartitionKey must be specified.");
			if (string.IsNullOrWhiteSpace(RowKey)) throw new Exception("The RowKey must be specified.");
			AzureStorageHelper.GetTableClient(azureStorageSettings, tableNames.TimerBot).UpsertEntity(this);
		}

		public static void Save(
			AzureStorageSettings azureStorageSettings,
			TableNames tableNames,
			string channelName,
			string botTimerName,
			string responseMessage,
			int onlineInterval,
			int offlineInterval,
			int chatLines,
			DateTime? lastExecution = null)
		{
			BotTimerEntity botTimerEntity = new()
			{
				PartitionKey = channelName,
				RowKey = botTimerName,
				ChannelName = channelName,
				BotTimerName = botTimerName,
				ResponseMessage = responseMessage,
				OnlineInterval = onlineInterval,
				OfflineInterval = offlineInterval,
				ChatLines = chatLines,
				LastExecution = (lastExecution == null) ? DateTime.UtcNow : (DateTime)lastExecution
			};
			botTimerEntity.Save(azureStorageSettings, tableNames);
		}

		public static void BotTimerExecuted(BotTimer botTimer, AzureStorageSettings azureStorageSettings, TableNames tableNames, DateTime? executionTime = null)
		{
			botTimer.LastExecution = (executionTime != null) ? (DateTime)executionTime : DateTime.UtcNow;
			FromBotTimer(botTimer).Save(azureStorageSettings, tableNames);
		}

		public static List<BotTimer> Retrieve(AzureStorageSettings azureStorageSettings, TableNames tableNames, string channelName)
		{
			return ToBotTimerList(AzureStorageHelper.GetTableClient(azureStorageSettings, tableNames.TimerBot).Query<BotTimerEntity>(t => t.PartitionKey == channelName.ToLower()).ToList());
		}

		private BotTimer ToBotTimer()
		{
			return new BotTimer()
			{
				ChannelName = this.ChannelName,
				BotTimerName = this.BotTimerName,
				ResponseMessage = this.ResponseMessage,
				OnlineInterval = this.OnlineInterval,
				OfflineInterval = this.OfflineInterval,
				ChatLines = this.ChatLines,
				LastExecution = this.LastExecution
			};
		}

		private static List<BotTimer> ToBotTimerList(List<BotTimerEntity> botTimerEntities)
		{
			List<BotTimer> botTimers = new();
			foreach (BotTimerEntity botTimerEntity in botTimerEntities)
			{
				botTimers.Add(botTimerEntity.ToBotTimer());
			}
			return botTimers;
		}

		private static BotTimerEntity FromBotTimer(BotTimer botTimer)
		{
			return new BotTimerEntity()
			{
				PartitionKey = botTimer.ChannelName,
				RowKey = botTimer.BotTimerName,
				BotTimerName = botTimer.BotTimerName,
				ResponseMessage = botTimer.ResponseMessage,
				OnlineInterval = botTimer.OnlineInterval,
				OfflineInterval = botTimer.OfflineInterval,
				ChatLines = botTimer.ChatLines,
				LastExecution = botTimer.LastExecution
			};
		}

	}

}