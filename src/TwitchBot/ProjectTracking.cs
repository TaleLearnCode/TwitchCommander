using Azure;
using Azure.Data.Tables;
using System;
using System.Linq;
using TwitchBot.settings;

namespace TwitchBot
{

	public class ProjectTracking : ITableEntity
	{

		private static AzureStorageSettings _azureStorageSettings;

		public string PartitionKey { get; set; }  // ChannelName
		public string RowKey { get; set; } // ProjectName
		public DateTimeOffset? Timestamp { get; set; }
		public ETag ETag { get; set; }

		public string ProjectName { get; set; }
		public int BricksDropped { get; set; }
		public int Oofs { get; set; }
		public int ProjectTimer { get; set; }

		public ProjectTracking() { }

		public ProjectTracking(string channelName, string projectName, AzureStorageSettings azureStorageSettings)
		{
			PartitionKey = channelName;
			RowKey = projectName.ToLower();
			ProjectName = projectName;
			_azureStorageSettings = azureStorageSettings;
		}

		private static TableClient GetTableClient(AzureStorageSettings azureStorageSettings)
		{
			TableClient tableClient;
			tableClient = new TableClient(new Uri(azureStorageSettings.Uri),
					"BricksWithChad",
					new TableSharedKeyCredential(azureStorageSettings.AccountName, azureStorageSettings.AccountKey));
			return tableClient;
		}

		private static TableClient GetTableClient()
		{
			return GetTableClient(_azureStorageSettings);
		}

		public static ProjectTracking Retrieve(string channelName, string projectName, AzureStorageSettings azureStorageSettings)
		{

			ProjectTracking projectTracking = GetTableClient(azureStorageSettings).Query<ProjectTracking>(s => s.PartitionKey == channelName && s.RowKey == projectName.ToLower()).FirstOrDefault();

			return projectTracking;

		}

		public void Save()
		{
			if (!string.IsNullOrWhiteSpace(PartitionKey) && !string.IsNullOrWhiteSpace(RowKey))
				GetTableClient().UpsertEntity(this);
		}

	}
}