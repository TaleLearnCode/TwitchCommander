using Azure;
using Azure.Data.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using TaleLearnCode.TwitchCommander.Exceptions;
using TaleLearnCode.TwitchCommander.Models;

namespace TaleLearnCode.TwitchCommander.AzureStorage
{
	public class ProjectTrackingEntity : ITableEntity
	{

		public string PartitionKey { get; set; }
		public string RowKey { get; set; }
		public DateTimeOffset? Timestamp { get; set; }
		public ETag ETag { get; set; }

		public string ChannelName { get; set; } // PartitionKey
		public string ProjectName { get; set; } // PartitionKey
		public string StreamId { get; set; }  // RowKey
		public string Details { get; set; }
		public int ElaspedSeconds { get; set; }
		public int DroppedBricks { get; set; }
		public int Oofs { get; set; }

		public ProjectTracking ToProjectTracking()
		{
			return new ProjectTracking()
			{
				ChannelName = this.ChannelName,
				ProjectName = this.ProjectName,
				StreamId = this.StreamId,
				Details = this.Details,
				ElaspedSeconds = this.ElaspedSeconds,
				DroppedBricks = this.DroppedBricks,
				Oofs = this.Oofs
			};
		}

		public static ProjectTrackingEntity FromProjectTracking(ProjectTracking projectTracking)
		{
			return new ProjectTrackingEntity()
			{
				PartitionKey = $"{projectTracking.ChannelName}|{projectTracking.ProjectName}",
				RowKey = projectTracking.StreamId,
				ChannelName = projectTracking.ChannelName,
				ProjectName = projectTracking.ProjectName,
				StreamId = projectTracking.StreamId,
				Details = projectTracking.Details,
				ElaspedSeconds = projectTracking.ElaspedSeconds,
				DroppedBricks = projectTracking.DroppedBricks,
				Oofs = projectTracking.Oofs
			};
		}

		public void Save(AzureStorageSettings azureStorageSettings, TableNames tableNames)
		{
			if (azureStorageSettings == null) throw new ArgumentNullException(nameof(azureStorageSettings));
			if (string.IsNullOrWhiteSpace(tableNames.ProjectTracking)) throw new SettingMissingException(nameof(tableNames.ProjectTracking));
			if (string.IsNullOrWhiteSpace(PartitionKey)) throw new Exception("The PartitionKey value must be specified.");
			if (string.IsNullOrWhiteSpace(RowKey)) throw new Exception("The RowKey value must be specified.");
			AzureStorageHelper.GetTableClient(azureStorageSettings, tableNames.ProjectTracking).UpsertEntity(this);
		}

		public static ProjectTracking Save(AzureStorageSettings azureStorageSettings, TableNames tableNames, ProjectTracking projectTracking)
		{
			ProjectTrackingEntity projectTrackingEntity = FromProjectTracking(projectTracking);
			projectTrackingEntity.Save(azureStorageSettings, tableNames);
			return projectTrackingEntity.ToProjectTracking();
		}

		public static IEnumerable<ProjectTracking> RetrieveForProject(AzureStorageSettings azureStorageSettings, TableNames tableNames, string channelName, string projectName)
		{
			return AzureStorageHelper.GetTableClient(azureStorageSettings, tableNames.ProjectTracking)
				.Query<ProjectTrackingEntity>(t => t.PartitionKey == GetPartitionKey(channelName, projectName)).ToList()
				.Select(l => l.ToProjectTracking());
		}

		public static ProjectTracking RetrieveForStream(AzureStorageSettings azureStorageSettings, TableNames tableNames, string channelName, string projectName, string streamId)
		{
			ProjectTrackingEntity results = AzureStorageHelper.GetTableClient(azureStorageSettings, tableNames.ProjectTracking)
				.Query<ProjectTrackingEntity>(t => t.PartitionKey == GetPartitionKey(channelName, projectName) && t.RowKey == streamId)
				.SingleOrDefault();
			if (results != null)
				return results.ToProjectTracking();
			else
				return null;
		}

		private static string GetPartitionKey(string channelName, string partitionKey)
		{
			return $"{channelName}|{partitionKey}";
		}

	}

}