﻿using Azure;
using Azure.Data.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using TaleLearnCode.TwitchCommander.Helpers;
using TaleLearnCode.TwitchCommander.Settings;

namespace TaleLearnCode.TwitchCommander.AzureStorage
{
	public class ChatCommandEntity : ITableEntity
	{

		/// <summary>
		/// The partition key is a unique identifier for the partition within a given table and forms the first part of an entity's primary key.
		/// </summary>
		/// <value>
		/// A string containing the partition key for the entity.
		/// </value>
		/// <remarks>The name of the channel the command is run on is used as the partition key.</remarks>
		public string PartitionKey { get; set; }

		/// <summary>
		/// The row key is a unique identifier for an entity within a given partition. Together the <see cref="P:Azure.Data.Tables.ITableEntity.PartitionKey" /> and RowKey uniquely identify every entity within a table.
		/// </summary>
		/// <value>
		/// A string containing the row key for the entity.
		/// </value>
		/// <remarks>The name of the command is used as the row key.</remarks>
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

		///// <summary>
		///// Gets or sets the name of the Twitch channel the command is associated with.
		///// </summary>
		///// <value>
		///// A <c>string</c> representing the Twitch channel name.
		///// </value>
		///// <remarks>This value is a facade to <see cref="PartitionKey"/>.</remarks>
		//public string ChannelName
		//{
		//	get { return PartitionKey; }
		//	set { PartitionKey = value; }
		//}

		//public string Command { get; set; }

		/// <summary>
		/// Gets or sets the name of the chat command.
		/// </summary>
		/// <value>
		/// A <see cref="string"/> representing the name of the chat command.
		/// </value>
		/// <remarks>This value is a facade to <see cref="RowKey"/>.</remarks>
		public string CommandName { get; set; }

		/// <summary>
		/// Gets or sets the user permission.
		/// </summary>
		/// <value>
		/// A <c>string</c> representing the minimum permission level required to execute the command.
		/// </value>
		public UserPermission UserPermission { get; set; }

		/// <summary>
		/// Gets or sets the template used for the formatted responses of the command.
		/// </summary>
		/// <value>
		/// A <c>string</c> representing the template to use when the command responds.
		/// </value>
		public string Response { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this command is enabled when streaming.
		/// </summary>
		/// <value>
		///   <c>true</c> if this command is enabled when streaming; otherwise, <c>false</c>.
		/// </value>
		public bool IsEnabledWhenStreaming { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this command is enabled when not streaming.
		/// </summary>
		/// <value>
		///   <c>true</c> if this command is enabled when not streaming; otherwise, <c>false</c>.
		/// </value>
		public bool IsEnabledWhenNotStreaming { get; set; }

		/// <summary>
		/// Gets or sets the type of the response the command uses.
		/// </summary>
		/// <value>
		/// A <see cref="CommandResponseType"/> representing the response type used by the command.
		/// </value>
		public CommandResponseType CommandResponseType { get; set; }

		/// <summary>
		/// Gets or sets the number of seconds to wait before allowing a user to use the command again.
		/// </summary>
		/// <value>
		/// A <c>int</c> representing the cooldown period for the user in seconds.
		/// </value>
		public int UserCooldown { get; set; }

		/// <summary>
		/// Gets or sets the number of seconds to wait before allowing anyone to use the command again.
		/// </summary>
		/// <value>
		/// A <c>int</c> representing the cooldown for everyone in seconds.
		/// </value>
		public int GlobalCooldown { get; set; }

		/// <summary>
		/// Gets or sets the aliases for the command text.
		/// </summary>
		/// <value>
		/// A pipe-delimited <see cref="string"/> representing the list of command aliases.
		/// </value>
		public string CommandAliases { get; set; }

		/// <summary>
		/// Retrieves all of the chat commands for the specified <paramref name="channelName"/>.
		/// </summary>
		/// <param name="channelName">Name of the channel whose chat commands to be retrieved.</param>
		/// <param name="azureStorageSettings">A <see cref="AzureStorageSettings"/> containing the Azure Storage connection details.</param>
		/// <returns>A <see cref="List{ChatCommand}"/> representing the list of chat commands for the channel.</returns>
		public static List<ChatCommand> Retrieve(string channelName, AzureStorageSettings azureStorageSettings)
		{
			return ToChatCommandList(AzureStorageHelper.GetTableClient(azureStorageSettings, "tableName").Query<ChatCommandEntity>(c => c.PartitionKey == channelName.ToLower()).ToList());
		}

		/// <summary>
		/// Retrieves the specified chat command.
		/// </summary>
		/// <param name="channelName">Name of the channel whose chat commands to be retrieved.</param>
		/// <param name="command">The command to be retrieved.</param>
		/// <param name="azureStorageSettings">A <see cref="AzureStorageSettings"/> containing the Azure Storage connection details.</param>
		/// <returns></returns>
		public static ChatCommand Retrieve(string channelName, string command, AzureStorageSettings azureStorageSettings)
		{
			var response = AzureStorageHelper.GetTableClient(azureStorageSettings, azureStorageSettings.ChatCommandTableName).Query<ChatCommandEntity>(c => c.PartitionKey == channelName.ToLower() && c.RowKey == command.ToLower()).FirstOrDefault();
			return ToChatCommand(response);
		}

		/// <summary>
		/// Saves the <see cref="ChatCommand"/> to the database.
		/// </summary>
		/// <param name="azureStorageSettings">A <see cref="AzureStorageSettings"/> containing the Azure Storage connection details.</param>
		public void Save(AzureStorageSettings azureStorageSettings)
		{
			if (!string.IsNullOrWhiteSpace(PartitionKey) && !string.IsNullOrWhiteSpace(RowKey))
				AzureStorageHelper.GetTableClient(azureStorageSettings, azureStorageSettings.ChatCommandTableName).UpsertEntity(this);
		}

		private static ChatCommand ToChatCommand(ChatCommandEntity input)
		{
			return new ChatCommand()
			{
				ChannelName = input.PartitionKey,
				Command = input.RowKey,
				CommandName = input.CommandName,
				UserPermission = input.UserPermission,
				Response = input.Response,
				IsEnabledWhenStreaming = input.IsEnabledWhenStreaming,
				IsEnabledWhenNotStreaming = input.IsEnabledWhenNotStreaming,
				CommandResponseType = input.CommandResponseType,
				UserCooldown = input.UserCooldown,
				GlobalCooldown = input.GlobalCooldown,
				CommandAliases = input.CommandAliases.Split('|').ToList()
			};
		}

		private static List<ChatCommand> ToChatCommandList(List<ChatCommandEntity> input)
		{
			List<ChatCommand> results = new();
			foreach (var entity in input)
				results.Add(ToChatCommand(entity));
			return results;
		}


	}

}