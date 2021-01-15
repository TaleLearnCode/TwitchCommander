﻿using System;
using System.Windows.Forms;
using TaleLearnCode.TwitchCommander.Events;
using TaleLearnCode.TwitchCommander.Extensions;

namespace TaleLearnCode.TwitchCommander.UserControls
{

	public partial class Main_Metrics : UserControl
	{

		private WOPR _wopr = null;

		public WOPR WOPR
		{
			get { return _wopr; }
			set
			{
				_wopr = value;
				if (_wopr != null) _wopr.OnProjectUpdated += WOPR_OnProjectUpdated;
			}
		}

		public void UpdateProjectTimer(TimeSpan projectTimer)
		{
			DisplayProjectTime(projectTimer);
		}

		public Main_Metrics()
		{
			InitializeComponent();
		}

		public void Disable()
		{
			DisplayProjectName(string.Empty);
			DisplayProjectTime(new TimeSpan(0));
			DisplaySubscriberCount(0);
			DisplayFollowerCount(0);
			DisplayViewerCount(0);
			DisplayDroppedBrickCount(0, 0);
			DisplayOofCount(0, 0);
		}

		private void WOPR_OnProjectUpdated(object sender, OnProjectUpdatedArgs e)
		{
			DisplayProjectName(e.ProjectName);
			DisplayProjectTime(e.ProjectTimer);
			if (DroppedBrickCount.Visible) DisplayDroppedBrickCount(e.DroppedBricks, e.OverallDroppedBricks);
			if (OofCount.Visible) DisplayOofCount(e.Oofs, e.OverallOofs);
		}

		private void SetProject_Click(object sender, EventArgs e)
		{
			if (WOPR != null)
				WOPR.SetProject(ProjectName.Text);
		}

		delegate void DisplayProjectNameCallback(string projectName);

		private void DisplayProjectName(string projectName)
		{
			if (ProjectName.InvokeRequired)
				Invoke(new DisplayProjectNameCallback(DisplayProjectName), new object[] { projectName });
			else
				ProjectName.Text = projectName;
		}

		delegate void DisplayProjectTimeCallback(TimeSpan projectTimer);

		private void DisplayProjectTime(TimeSpan projectTimer)
		{
			if (ProjectTime.InvokeRequired)
				Invoke(new DisplayProjectTimeCallback(DisplayProjectTime), new object[] { projectTimer });
			else
				ProjectTime.Text = projectTimer.ToString(@"hh\:mm\:ss");
		}

		delegate void DisplaySubscriberCountCallback(int subscriberCount);

		private void DisplaySubscriberCount(int subscriberCount)
		{
			if (SubscriberCount.InvokeRequired)
				Invoke(new DisplaySubscriberCountCallback(DisplaySubscriberCount), new object[] { subscriberCount });
			else
				SubscriberCount.Text = subscriberCount.Display();
		}

		delegate void DisplayFollowerCountCallback(int followerCount);

		private void DisplayFollowerCount(int followerCount)
		{
			if (FollwerCount.InvokeRequired)
				Invoke(new DisplayFollowerCountCallback(DisplayFollowerCount), new object[] { followerCount });
			else
				FollwerCount.Text = followerCount.Display();
		}

		delegate void DisplayViewerCountCallback(int viewerCount);

		private void DisplayViewerCount(int viewerCount)
		{
			if (ViewerCount.InvokeRequired)
				Invoke(new DisplayViewerCountCallback(DisplayViewerCount), new object[] { viewerCount });
			else
				ViewerCount.Text = viewerCount.Display();
		}

		delegate void DisplayDroppedBrickCountCallback(int currentDroppedBricks, int overallDroppedBricks);

		private void DisplayDroppedBrickCount(int currentDroppedBricks, int overallDroppedBricks)
		{
			if (DroppedBrickCount.InvokeRequired)
				Invoke(new DisplayDroppedBrickCountCallback(DisplayDroppedBrickCount), new object[] { currentDroppedBricks, overallDroppedBricks });
			else
				DroppedBrickCount.Text = $"{currentDroppedBricks.Display()} / {overallDroppedBricks.Display()}";
		}

		delegate void DisplayOofCountCallback(int currentOffs, int overallOofs);

		private void DisplayOofCount(int currentOofs, int overallOofs)
		{
			if (OofCount.InvokeRequired)
				Invoke(new DisplayOofCountCallback(DisplayOofCount), new object[] { currentOofs, overallOofs });
			else
				OofCount.Text = $"{currentOofs.Display()} / {overallOofs.Display()}";
		}

	}

}