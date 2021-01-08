using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using TaleLearnCode.TwitchCommander.Events;
using TaleLearnCode.TwitchCommander.Models;
using Telerik.WinControls.UI;

namespace TaleLearnCode.TwitchCommander
{
	public partial class Main : RadForm
	{

		WOPR _BricksWithChad;
		WOPR _TaleLearlCode;

		private IConfigurationRoot _config;
		private AppSettings _appSettings = new();
		private AzureStorageSettings _azureStorageSettings = new();
		private TableNames _tableNames = new();
		private TwitchSettings _bricksWithChadSettings = new();
		private TwitchSettings _taleLearnCodeSettings = new();

		public Main()
		{
			InitializeComponent();

			TwitchClientLog.AutoSizeItems = true;

			GetSettings();
			InitializeBricksWithChad();
			InitializeTaleLearnCode();
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

		private void Connect(WOPR wopr, RadButton connectButton)
		{
			if (connectButton.Text == "Connect")
			{
				wopr.Connect();
				connectButton.Text = "Disconnect";
			}
			else
			{
				wopr.Disconnect();
				connectButton.Text = "Connect";
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
			_BricksWithChad = new(_appSettings, _azureStorageSettings, _tableNames, _bricksWithChadSettings, true);
			_BricksWithChad.OnLoggedEvent += BricksWithChad_OnLoggedEvent;
		}

		private void BricksWithChad_OnLoggedEvent(object sender, OnLoggedEventArgs e)
		{
			DisplayLoggedEvent(e, Properties.Resources.BricksWithChad);
		}

		private void ConnectBricksWithChad_Click(object sender, EventArgs e)
		{
			Connect(_BricksWithChad, ConnectBricksWithChad);
		}

		#endregion

		#region TaleLearnCode

		private void InitializeTaleLearnCode()
		{
			_TaleLearlCode = new(_appSettings, _azureStorageSettings, _tableNames, _taleLearnCodeSettings, true);
			_TaleLearlCode.OnLoggedEvent += TaleLearnCode_OnLoggedEvent;
		}

		private void TaleLearnCode_OnLoggedEvent(object sender, OnLoggedEventArgs e)
		{
			DisplayLoggedEvent(e, Properties.Resources.TaleLearnCode);
		}

		private void ConnectTaleLearnCode_Click(object sender, EventArgs e)
		{
			Connect(_TaleLearlCode, ConnectTaleLearnCode);
		}

		#endregion

	}

}