using System;
using System.Collections.Generic;
using System.Linq;
using TaleLearnCode.TwitchCommander.AzureStorage;

namespace TaleLearnCode.TwitchCommander.Models
{

	/// <summary>
	/// Represents the tracking information for a project for a stream.
	/// </summary>
	/// <seealso cref="IProjectTracking" />
	/// <inheritdoc/>
	public class ProjectTracking
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

		public static ProjectTracking Retrieve(AzureStorageSettings azureStorageSettings, TableNames tableNames, string channelName, string projectName, string streamId)
		{

			ProjectTracking projectTracking = new()
			{
				ChannelName = channelName,
				ProjectName = projectName,
				StreamId = streamId
			};

			List<ProjectTracking> projectTrackingRecords = ProjectTrackingEntity.RetrieveForProject(azureStorageSettings, tableNames, channelName, projectName).ToList();
			if (!projectTrackingRecords.Any(t => t.StreamId == streamId))
			{
				projectTrackingRecords.Add(ProjectTrackingEntity.Save(azureStorageSettings, tableNames, projectTracking));
			}

			foreach (ProjectTracking projectTrackingRecord in projectTrackingRecords)
			{
				projectTracking.OverallDroppedBricks += projectTrackingRecord.DroppedBricks;
				projectTracking.OverallOofs += projectTrackingRecord.Oofs;
				projectTracking.OverallElapsedTime = projectTracking.OverallElapsedTime.Add(new TimeSpan(0, 0, projectTrackingRecord.ElaspedSeconds));

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