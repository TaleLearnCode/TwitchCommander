using Microsoft.Extensions.Configuration;
using System;
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

		OBSController _obsController = new();

		private IConfigurationRoot _config;
		private readonly AppSettings _appSettings = new();
		private readonly AzureStorageSettings _azureStorageSettings = new();
		private readonly TableNames _tableNames = new();
		private readonly TwitchSettings _bricksWithChadSettings = new();
		private readonly TwitchSettings _taleLearnCodeSettings = new();

		public Main()
		{
			InitializeComponent();
			GetSettings();
			InitializeTaleLearnCode();
			InitializeBricksWithChad();
			FormSetup();
			timer.Enabled = true;
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
			// HACK: Need to control this via the form
			_obsController.Connect(_appSettings.OBSURL, _appSettings.OBSPassword);
		}

		private async void ViewSelector_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
		{
			if (ViewSelector.SelectedIndex > -1)
			{

				WOPR wopr = null;
				if (ViewSelector.Items[ViewSelector.SelectedIndex].Text == _bricksWithChad.ChannelName)
					wopr = _bricksWithChad;
				else if (ViewSelector.Items[ViewSelector.SelectedIndex].Text == _taleLearnCode.ChannelName)
					wopr = _taleLearnCode;

				ProjectInfo.Enabled = true;
				ProjectInfo.WOPR = wopr;

				StreamStats.Enabled = true;
				StreamStats.WOPR = wopr;

				OBSStatus.Enabled = true;
				OBSStatus.OBSControler = _obsController;

				ActivityFeedLabel.Enabled = true;
				ActivityFeed.Enabled = true;


				AlertsGroup.Enabled = true;
				OBSSettingsGroup.Enabled = true;
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

		#region BricksWithChad

		private void InitializeBricksWithChad()
		{
			_bricksWithChad = new("BricksWithChad", _appSettings, _azureStorageSettings, _tableNames, _bricksWithChadSettings, true);
			_bricksWithChad.OnLoggedEvent += BricksWithChad_OnLoggedEvent;
		}

		private void BricksWithChad_OnLoggedEvent(object sender, OnLoggedEventArgs e)
		{
			DisplayLoggedEvent(e, Properties.Resources.BricksWithChad);
		}

		private void ConnectedToBricksWithChad_ValueChanged(object sender, System.EventArgs e)
		{
			if (ConnectedToBricksWithChad.Value)
				_bricksWithChad.Connect(FakeOnline.Checked);
			else
				_bricksWithChad.Disconnect();
		}

		#endregion

		#region TaleLearnCode

		private void InitializeTaleLearnCode()
		{
			_taleLearnCode = new("TaleLearnCode", _appSettings, _azureStorageSettings, _tableNames, _taleLearnCodeSettings, true);
			_taleLearnCode.OnLoggedEvent += TaleLearnCode_OnLoggedEvent;
		}

		private void TaleLearnCode_OnLoggedEvent(object sender, OnLoggedEventArgs e)
		{
			DisplayLoggedEvent(e, Properties.Resources.TaleLearnCode);
		}

		private void ConnectedToTaleLearnCode_ValueChanged(object sender, System.EventArgs e)
		{
			if (ConnectedToTaleLearnCode.Value)
				_taleLearnCode.Connect(FakeOnline.Checked);
			else
				_taleLearnCode.Disconnect();
		}

		#endregion

		private void SetOBSSceneCollection_Click(object sender, System.EventArgs e)
		{

		}

		//private void SetProject_Click(object sender, EventArgs e)
		//{
		//	if (ViewSelector.Items[ViewSelector.SelectedIndex].Text == _bricksWithChad.ChannelName)
		//	{
		//		_bricksWithChad.SetProject(ProjectName.Text);
		//	}
		//	else if (ViewSelector.Items[ViewSelector.SelectedIndex].Text == _taleLearnCode.ChannelName)
		//	{
		//		_taleLearnCode.SetProject(ProjectName.Text);
		//	}
		//}

		private void timer_Tick(object sender, EventArgs e)
		{
			if (ViewSelector.SelectedIndex > -1)
				if (_bricksWithChad.ProjectTracking != null && ViewSelector.Items[ViewSelector.SelectedIndex].Text == _bricksWithChad.ChannelName)
				{
					ProjectInfo.UpdateProjectTimer(_bricksWithChad.ProjectTracking.OverallElapsedTime);
				}
				else if (_taleLearnCode.ProjectTracking != null && ViewSelector.Items[ViewSelector.SelectedIndex].Text == _taleLearnCode.ChannelName)
				{
					ProjectInfo.UpdateProjectTimer(_taleLearnCode.ProjectTracking.OverallElapsedTime);
				}
		}

		private void button1_Click(object sender, EventArgs e)
		{
			_obsController.Connect(_appSettings.OBSURL, _appSettings.OBSPassword);
			//_obsController.TestMe();
			_obsController.ShowSceneItem("SocketsTest", "Media Source: SocketsTest", 16);
		}
	}
}