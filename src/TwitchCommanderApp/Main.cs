using Microsoft.Extensions.Configuration;
using System;
using System.Globalization;
using System.IO;
using TaleLearnCode.TwitchCommander.Events;
using TaleLearnCode.TwitchCommander.Models;
using Telerik.WinControls.UI;

namespace TaleLearnCode.TwitchCommander
{
	public partial class Main : Telerik.WinControls.UI.RadForm
	{

		WOPR _bricksWithChad;
		WOPR _taleLearnCode;

		private IConfigurationRoot _config;
		private readonly AppSettings _appSettings = new();
		private readonly AzureStorageSettings _azureStorageSettings = new();
		private readonly TableNames _tableNames = new();
		private readonly TwitchSettings _bricksWithChadSettings = new();
		private readonly TwitchSettings _taleLearnCodeSettings = new();

		public Main()
		{
			InitializeComponent();
			//GetSettings();
			//InitializeTaleLearnCode();
			//InitializeBricksWithChad();
			//FormSetup();
			//timer.Enabled = true;
		}

		private void GetSettings()
		{
			var builder = new ConfigurationBuilder()
				.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "Settings"))
				.AddJsonFile("AppSettings.json", optional: false, reloadOnChange: true)
				.AddJsonFile("AzureStorage.json", optional: false, reloadOnChange: true)
				.AddJsonFile("BricksWithChad.json", optional: false, reloadOnChange: true)
				.AddJsonFile("TableNames.json", optional: false, reloadOnChange: true)
				.AddJsonFile("TaleLearnCode.json", optional: false, reloadOnChange: true);
			_config = builder.Build();

			_config.GetSection("AppSettings").Bind(_appSettings);
			_config.GetSection("AzureStorage").Bind(_azureStorageSettings);
			_config.GetSection("TableNames").Bind(_tableNames);
			_config.GetSection("BricksWithChad").Bind(_bricksWithChadSettings);
			_config.GetSection("TaleLearnCode").Bind(_taleLearnCodeSettings);
		}

		private void FormSetup()
		{
			ViewSelector.Items.Add(_bricksWithChad.ChannelName);
			ViewSelector.Items.Add(_taleLearnCode.ChannelName);
		}

		private void ViewSelector_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
		{
			if (ViewSelector.SelectedIndex > -1)
			{
				ActivityFeedLabel.Enabled = true;
				ActivityFeed.Enabled = true;
				MetricsGroup.Enabled = true;
				AlertsGroup.Enabled = true;
				OBSSettingsGroup.Enabled = true;
				StreamStatsGroup.Enabled = true;
				OBSStatsGroup.Enabled = true;
			}
		}

		delegate void DisplayLoggedEventCallback(OnLoggedEventArgs onLoggedEventArgs, System.Drawing.Image iconImage);

		private void DisplayLoggedEvent(OnLoggedEventArgs onLoggedEventArgs, System.Drawing.Image iconImage)
		{
			if (TwitchClientLog.InvokeRequired)
			{
				DisplayLoggedEventCallback d = new DisplayLoggedEventCallback(DisplayLoggedEvent);
				Invoke(d, new object[] { onLoggedEventArgs, iconImage });
			}
			else
			{
				DescriptionTextListDataItem descriptionItem = new();
				descriptionItem.Text = $"[{onLoggedEventArgs.DateTime}] {onLoggedEventArgs.BotUsername}";
				descriptionItem.DescriptionText = $"[{onLoggedEventArgs.DateTime}] {onLoggedEventArgs.Data}";
				descriptionItem.Image = iconImage;
				TwitchClientLog.Items.Add(descriptionItem);
				TwitchClientLog.ScrollToItem(descriptionItem);
			}
		}

		#region Metrics Group

		delegate void DisplayProjectNameCallback(string projectName);

		public void DisplayProjectName(string projectName)
		{
			if (ProjectName.InvokeRequired)
				Invoke(new DisplayProjectNameCallback(DisplayProjectName), new object[] { projectName });
			else
				ProjectName.Text = projectName;
		}

		delegate void DisplayProjectTimeCallback(TimeSpan projectTimer);

		public void DisplayProjectTime(TimeSpan projectTimer)
		{
			if (ProjectTime.InvokeRequired)
				Invoke(new DisplayProjectTimeCallback(DisplayProjectTime), new object[] { projectTimer });
			else
				ProjectTime.Text = projectTimer.ToString(@"hh\:mm\:ss");
		}

		delegate void DisplaySubscriberCountCallback(int subscriberCount);

		public void DisplaySubscriberCount(int subscriberCount)
		{
			if (SubscriberCount.InvokeRequired)
				Invoke(new DisplaySubscriberCountCallback(DisplaySubscriberCount), new object[] { subscriberCount });
			else
				SubscriberCount.Text = PrintNumber(subscriberCount);
		}

		delegate void DisplayFollowerCountCallback(int followerCount);

		public void DisplayFollowerCount(int followerCount)
		{
			if (FollwerCount.InvokeRequired)
				Invoke(new DisplayFollowerCountCallback(DisplayFollowerCount), new object[] { followerCount });
			else
				FollwerCount.Text = PrintNumber(followerCount);
		}

		delegate void DisplayViewerCountCallback(int viewerCount);

		public void DisplayViewerCount(int viewerCount)
		{
			if (ViewerCount.InvokeRequired)
				Invoke(new DisplayViewerCountCallback(DisplayViewerCount), new object[] { viewerCount });
			else
				ViewerCount.Text = PrintNumber(viewerCount);
		}

		delegate void DisplayDroppedBrickCountCallback(int currentDroppedBricks, int overallDroppedBricks);

		public void DisplayDroppedBrickCount(int currentDroppedBricks, int overallDroppedBricks)
		{
			if (DroppedBrickCount.InvokeRequired)
				Invoke(new DisplayDroppedBrickCountCallback(DisplayDroppedBrickCount), new object[] { currentDroppedBricks, overallDroppedBricks });
			else
				DroppedBrickCount.Text = $"{PrintNumber(currentDroppedBricks)} / {PrintNumber(overallDroppedBricks)}";
		}

		delegate void DisplayOofCountCallback(int currentOffs, int overallOofs);

		public void DisplayOofCount(int currentOofs, int overallOofs)
		{
			if (OofCount.InvokeRequired)
				Invoke(new DisplayOofCountCallback(DisplayOofCount), new object[] { currentOofs, overallOofs });
			else
				OofCount.Text = $"{PrintNumber(currentOofs)} / {PrintNumber(overallOofs)}";
		}

		private void DisplayProjectInformation(OnProjectUpdatedArgs onProjectUpdatedArgs)
		{
			DisplayProjectName(onProjectUpdatedArgs.ProjectName);
			DisplayProjectTime(onProjectUpdatedArgs.ProjectTimer);
			if (DroppedBrickCount.Visible) DisplayDroppedBrickCount(onProjectUpdatedArgs.DroppedBricks, onProjectUpdatedArgs.OverallDroppedBricks);
			if (OofCount.Visible) DisplayOofCount(onProjectUpdatedArgs.Oofs, onProjectUpdatedArgs.OverallOofs);
		}

		#endregion


		#region BricksWithChad

		private void InitializeBricksWithChad()
		{
			_bricksWithChad = new("BricksWithChad", _appSettings, _azureStorageSettings, _tableNames, _bricksWithChadSettings, true);
			_bricksWithChad.OnLoggedEvent += BricksWithChad_OnLoggedEvent;
			_bricksWithChad.OnProjectUpdated += BricksWithChad_OnProjectUpdated;
		}

		private void BricksWithChad_OnProjectUpdated(object sender, OnProjectUpdatedArgs e)
		{
			DisplayProjectInformation(e);
		}

		private void BricksWithChad_OnLoggedEvent(object sender, OnLoggedEventArgs e)
		{
			DisplayLoggedEvent(e, Properties.Resources.BricksWithChad);
		}

		private void ConnectedToBricksWithChad_ValueChanged(object sender, System.EventArgs e)
		{
			if (ConnectedToBricksWithChad.Value)
				_bricksWithChad.Connect();
			else
				_bricksWithChad.Disconnect();
		}

		#endregion

		#region TaleLearnCode

		private void InitializeTaleLearnCode()
		{
			_taleLearnCode = new("TaleLearnCode", _appSettings, _azureStorageSettings, _tableNames, _taleLearnCodeSettings, true);
			_taleLearnCode.OnLoggedEvent += TaleLearnCode_OnLoggedEvent;
			_taleLearnCode.OnProjectUpdated += TaleLearnCode_OnProjectUpdated;
		}

		private void TaleLearnCode_OnProjectUpdated(object sender, OnProjectUpdatedArgs e)
		{
			DisplayProjectInformation(e);
		}

		private void TaleLearnCode_OnLoggedEvent(object sender, OnLoggedEventArgs e)
		{
			DisplayLoggedEvent(e, Properties.Resources.TaleLearnCode);
		}

		private void ConnectedToTaleLearnCode_ValueChanged(object sender, System.EventArgs e)
		{
			if (ConnectedToTaleLearnCode.Value)
				_taleLearnCode.Connect();
			else
				_taleLearnCode.Disconnect();
		}

		#endregion

		#region Private Utility Methods

		private string PrintNumber(int number)
		{
			return number.ToString("N0", CultureInfo.InvariantCulture);
		}

		#endregion

		private void SetOBSSceneCollection_Click(object sender, System.EventArgs e)
		{

		}

		private void SetProject_Click(object sender, EventArgs e)
		{
			if (ViewSelector.Items[ViewSelector.SelectedIndex].Text == _bricksWithChad.ChannelName)
			{
				_bricksWithChad.SetProject(ProjectName.Text);
			}
			else if (ViewSelector.Items[ViewSelector.SelectedIndex].Text == _taleLearnCode.ChannelName)
			{
				_taleLearnCode.SetProject(ProjectName.Text);
			}
		}

		private void timer_Tick(object sender, EventArgs e)
		{
			if (ViewSelector.SelectedIndex > -1)
				if (_bricksWithChad.ProjectTracking != null && ViewSelector.Items[ViewSelector.SelectedIndex].Text == _bricksWithChad.ChannelName)
					DisplayProjectTime(_bricksWithChad.ProjectTracking.OverallElapsedTime);
				else if (_taleLearnCode.ProjectTracking != null && ViewSelector.Items[ViewSelector.SelectedIndex].Text == _taleLearnCode.ChannelName)
					DisplayProjectTime(_taleLearnCode.ProjectTracking.OverallElapsedTime);
		}
	}
}