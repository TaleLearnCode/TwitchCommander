using Microsoft.Extensions.Configuration;
using System.IO;
using System.Windows.Forms;
using TaleLearnCode.TwitchCommander.Events;
using TaleLearnCode.TwitchCommander.Models;
using Telerik.WinControls.Enumerations;
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
			GetSettings();
			InitializeTaleLearnCode();
			InitializeBricksWithChad();
			FormSetup();
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
				MessageBox.Show(ViewSelector.SelectedItem.Text);
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

	}
}