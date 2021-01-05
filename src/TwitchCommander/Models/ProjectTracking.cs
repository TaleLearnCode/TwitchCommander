using System;
using System.Collections.Generic;
using System.Linq;
using TaleLearnCode.TwitchCommander.AzureStorage;
using TaleLearnCode.TwitchCommander.Settings;

namespace TaleLearnCode.TwitchCommander.Models
{


	/// <summary>
	/// Represents the tracking information for a project for a stream.
	/// </summary>
	/// <seealso cref="IProjectTracking" />
	/// <inheritdoc/>
	public class ProjectTracking : IProjectTracking
	{

		public string ChannelName { get; set; }

		public string ProjectName { get; set; }

		public string StreamId { get; set; }

		public string Details { get; set; }

		public int ElaspedSeconds { get; set; }

		public TimeSpan OverallElapsedTime { get; set; }

		public int DroppedBricks { get; set; }

		public int OverallDroppedBricks { get; set; }

		public int Oofs { get; set; }

		public int OverallOofs { get; set; }

		public AzureStorageSettings AzureStorageSettings { get; set; }

		public ProjectTracking() { }

		public ProjectTracking(AzureStorageSettings azureStorageSettings)
		{
			AzureStorageSettings = azureStorageSettings;
		}

		public void Save()
		{
			ProjectTrackingEntity.Save(AzureStorageSettings, this);
		}

		public static ProjectTracking Retrieve(AzureStorageSettings azureStorageSettings, string channelName, string projectName, string streamId)
		{

			ProjectTracking projectTracking = new()
			{
				ChannelName = channelName,
				ProjectName = projectName,
				StreamId = streamId
			};

			List<ProjectTracking> projectTrackingRecords = ProjectTrackingEntity.RetrieveForProject(azureStorageSettings, channelName, projectName).ToList();
			if (!projectTrackingRecords.Any(t => t.StreamId == streamId))
			{
				projectTrackingRecords.Add(ProjectTrackingEntity.Save(azureStorageSettings, projectTracking));
			}

			foreach (ProjectTracking projectTrackingRecord in projectTrackingRecords)
			{
				projectTracking.OverallDroppedBricks += projectTrackingRecord.DroppedBricks;
				projectTracking.OverallOofs += projectTrackingRecord.Oofs;
				projectTracking.OverallElapsedTime.Add(new TimeSpan(0, 0, projectTrackingRecord.ElaspedSeconds));

				if (projectTrackingRecord.StreamId == projectTracking.StreamId)
				{
					projectTracking.DroppedBricks = projectTrackingRecord.DroppedBricks;
					projectTracking.Oofs = projectTrackingRecord.Oofs;
					projectTracking.ElaspedSeconds = projectTrackingRecord.ElaspedSeconds;
				}
			}

			return projectTracking;

		}

	}


}