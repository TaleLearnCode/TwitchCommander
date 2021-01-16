using System;
using TaleLearnCode.TwitchCommander.Models;

namespace TaleLearnCode.TwitchCommander.Events
{

	public class OnProjectUpdatedArgs : EventArgs
	{

		public string ProjectName { get; set; }

		public string Details { get; set; }

		public TimeSpan ProjectTimer { get; set; }

		public int DroppedBricks { get; set; }

		public int OverallDroppedBricks { get; set; }

		public int Oofs { get; set; }

		public int OverallOofs { get; set; }

		public OnProjectUpdatedArgs(ProjectTracking projectTracking)
		{
			ProjectName = projectTracking.ProjectName;
			Details = projectTracking.Details;
			ProjectTimer = projectTracking.OverallElapsedTime;
			DroppedBricks = projectTracking.DroppedBricks;
			OverallDroppedBricks = projectTracking.OverallDroppedBricks;
			Oofs = projectTracking.Oofs;
			OverallOofs = projectTracking.OverallOofs;
		}

	}

}