using Azure;
using Azure.Data.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using TaleLearnCode.TwitchCommander.Helpers;
using TaleLearnCode.TwitchCommander.Settings;

namespace TaleLearnCode.TwitchCommander.AzureStorage
{
	public class ChatCommandAliasEntity : ITableEntity
	{
		public string PartitionKey { get; set; } // Channel Name
		public string RowKey { get; set; } // Command Alias
		public DateTimeOffset? Timestamp { get; set; }
		public ETag ETag { get; set; }

		public string Command { get; set; }

		public ChatCommandAliasEntity() : base() { }

		public ChatCommandAliasEntity(string channelName, string command, string commandAlias)
		{
			PartitionKey = channelName.ToLower();
			RowKey = commandAlias.ToLower();
			Command = command.ToLower();
		}

		public void Save(AzureStorageSettings azureStorageSettings)
		{
			if (!string.IsNullOrWhiteSpace(PartitionKey) && !string.IsNullOrWhiteSpace(RowKey))
				AzureStorageHelper.GetTableClient(azureStorageSettings, azureStorageSettings.ChatCommandAliasesTableName).UpsertEntity(this);
		}

		public static List<ChatCommandAliasEntity> RetrieveForCommand(string channelName, string command, AzureStorageSettings azureStorageSettings)
		{
			return AzureStorageHelper.GetTableClient(azureStorageSettings, azureStorageSettings.ChatCommandAliasesTableName).Query<ChatCommandAliasEntity>(c => c.PartitionKey == channelName.ToLower() && c.Command == command.ToLower()).ToList();
		}

		public static ChatCommandAliasEntity Retrieve(string channelName, string commandAlias, AzureStorageSettings azureStorageSettings)
		{
			return AzureStorageHelper.GetTableClient(azureStorageSettings, azureStorageSettings.ChatCommandAliasesTableName).Query<ChatCommandAliasEntity>(c => c.PartitionKey == channelName.ToLower() && c.RowKey == commandAlias.ToLower()).SingleOrDefault();
		}

	}

}