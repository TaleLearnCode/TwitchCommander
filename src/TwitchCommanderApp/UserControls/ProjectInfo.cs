using System;
using System.Windows.Forms;
using TaleLearnCode.TwitchCommander.Events;
using TaleLearnCode.TwitchCommander.Extensions;
using TaleLearnCode.TwitchCommander.Models;

namespace TaleLearnCode.TwitchCommander.UserControls
{

	public partial class ProjectInfo : UserControl
	{

		private WOPR _wopr = null;

		public WOPR WOPR
		{
			get { return _wopr; }
			set
			{
				_wopr = value;
				if (_wopr != null)
				{
					_wopr.OnProjectUpdated += WOPR_OnProjectUpdated;
					_wopr.OnOof += WOPR_Oof;
					_wopr.OnBrickDropped += WOPR_BrickDropped;
				}
			}
		}

		public OBSController OBSController { get; set; }

		public AlertSetting BrickDropAlert { get; set; }

		public AlertSetting OofAlert { get; set; }

		private void WOPR_BrickDropped(object sender, OnBrickDropArgs e)
		{
			DisplayDroppedBricks(e.BricksDroppedDuringStream, e.BricksDroppedDuringProject);
			if (BrickDropAlert != null && OBSController != null && OBSController.DoesCurrentSceneContainSource(BrickDropAlert.AlertItem))
				OBSController.ShowSceneItem(BrickDropAlert.AlertItem, BrickDropAlert.AlertLength);
		}

		private void WOPR_Oof(object sender, OnOofArgs e)
		{
			DisplayOofs(e.OofsDuringStream, e.OofsDuringProject);
			if (OofAlert != null && OBSController != null && OBSController.DoesCurrentSceneContainSource(OofAlert.AlertItem))
				OBSController.ShowSceneItem(OofAlert.AlertItem, OofAlert.AlertLength);
		}

		public void UpdateProjectTimer(TimeSpan projectTimer)
		{
			ProjectTime.Text = FormatTimerOutput(projectTimer);
			OBSController.SetText("Text: Project Timer", FormatTimerOutput(projectTimer));
		}

		public void Disable()
		{
			ProjectName.Text = string.Empty;
			ProjectTime.Text = (new TimeSpan(0)).ToString(@"hh\:mm\:ss");
			if (DroppedBrickCount.Visible)
				DroppedBrickCount.Text = "0 / 0";
			if (OofCount.Visible)
				OofCount.Text = "0 / 0";
		}

		private void WOPR_OnProjectUpdated(object sender, OnProjectUpdatedArgs e)
		{
			DisplayProjectInfo(e);
		}

		private void SetProject_Click(object sender, EventArgs e)
		{
			if (WOPR != null)
				WOPR.SetProject(ProjectName.Text);
		}

		delegate void DisplayProjectInfoCallback(OnProjectUpdatedArgs onProjectUpdatedArgs);

		private void DisplayProjectInfo(OnProjectUpdatedArgs onProjectUpdatedArgs)
		{
			if (ProjectName.InvokeRequired)
			{
				Invoke(new DisplayProjectInfoCallback(DisplayProjectInfo), new object[] { onProjectUpdatedArgs });
			}
			else
			{
				ProjectName.Text = onProjectUpdatedArgs.ProjectName;
				ProjectTime.Text = FormatTimerOutput(onProjectUpdatedArgs.ProjectTimer);

				if (OBSController != null)
				{
					OBSController.SetText("Text: New Project Name", onProjectUpdatedArgs.ProjectName);
					OBSController.SetText("Text: Project Timer", FormatTimerOutput(onProjectUpdatedArgs.ProjectTimer));
				}

				DisplayOofs(onProjectUpdatedArgs.Oofs, onProjectUpdatedArgs.OverallOofs);
				DisplayDroppedBricks(onProjectUpdatedArgs.DroppedBricks, onProjectUpdatedArgs.OverallDroppedBricks);
			}
		}

		delegate void DisplayOofsCallback(int streamOofs, int projectOofs);

		private void DisplayOofs(int streamOofs, int projectOofs)
		{
			if (OofCount.InvokeRequired)
			{
				Invoke(new DisplayOofsCallback(DisplayOofs), new object[] { streamOofs, projectOofs });
			}
			else
			{
				if (OofCount.Visible)
					OofCount.Text = $"{streamOofs.Display()} / {projectOofs.Display()}";
				if (OBSController != null)
					OBSController.SetText("Text: Oofs", $"{streamOofs.Display()} / {projectOofs.Display()}");
			}
		}

		delegate void DisplayDroppedBricksCallback(int streamBricksDropped, int projectBricksDropped);

		private void DisplayDroppedBricks(int streamBricksDropped, int projectBricksDropped)
		{
			if (DroppedBrickCount.InvokeRequired)
			{
				Invoke(new DisplayDroppedBricksCallback(DisplayDroppedBricks), new object[] { streamBricksDropped, projectBricksDropped });
			}
			else
			{
				if (DroppedBrickCount.Visible)
					DroppedBrickCount.Text = $"{streamBricksDropped.Display()} / {projectBricksDropped.Display()}";
				if (OBSController != null)
					OBSController.SetText("Text: Dropped Bricks", $"{streamBricksDropped.Display()} / {projectBricksDropped.Display()}");
			}
		}

		private string FormatTimerOutput(TimeSpan timeSpan)
		{
			return timeSpan.ToString(@"hh\:mm\:ss");
		}

		public ProjectInfo()
		{
			InitializeComponent();
		}

	}

}