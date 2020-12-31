using System;
using System.Collections.Generic;
using TaleLearnCode.TwitchCommander.Settings;
using TwitchLib.Api;
using TwitchLib.Api.Services;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;

namespace TaleLearnCode.TwitchCommander
{

	public partial class WOPR
	{

		private readonly TwitchSettings _twitchSettings;
		private readonly AzureStorageSettings _azureStorageSettings;
		private readonly bool _viewLogs;

		private readonly TwitchClient _twitchClient = new();
		private ConnectionCredentials _credentials;
		private TwitchAPI _twitchAPI;
		private LiveStreamMonitorService _twitchMonitor;

		private bool _IsOnline;

		/// <summary>
		/// Initializes a new instance of the <see cref="WOPR"/> class.
		/// </summary>
		/// <param name="twitchSettings">A <see cref="TwitchSettings"/> representing the settings to use for connecting to Twitch.</param>
		/// <param name="azureStorageSettings">A <see cref="AzureStorageSettings"/> representing the settings to use for connecting to Azure Storage.</param>
		/// <param name="viewLogs">If set to <c>true</c> the Twitch logs will be provided to the consumer.</param>
		public WOPR(
			TwitchSettings twitchSettings,
			AzureStorageSettings azureStorageSettings,
			bool viewLogs)
		{

			_twitchSettings = twitchSettings;
			_azureStorageSettings = azureStorageSettings;
			_viewLogs = viewLogs;

			ConfigureTwitchClient();
			ConfigureTwitchAPI();
			ConfigureTwitchMonitor();
		}

		/// <summary>
		/// Starts monitoring Twitch in order to perform the necessary actions.
		/// </summary>
		public void Start()
		{
			_twitchClient.Connect();
			_twitchMonitor.Start();
			ConfigureTimers();
		}

		/// <summary>
		/// Configures the <see cref="TwitchClient"/> instance.
		/// </summary>
		private void ConfigureTwitchClient()
		{
			if (_viewLogs) _twitchClient.OnLog += TwitchClient_OnLog;
			_twitchClient.OnConnected += TwitchClient_OnConnected;
			_twitchClient.OnDisconnected += TwitchClient_OnDisconnected;
			_twitchClient.OnChatCommandReceived += TwitchClient_OnChatCommandReceived;
			_twitchClient.OnNewSubscriber += TwitchClient_OnNewSubscriber;
			_twitchClient.OnCommunitySubscription += TwitchClient_OnCommunitySubscription;
			_twitchClient.OnGiftedSubscription += TwitchClient_OnGiftedSubscription;
			_twitchClient.OnReSubscriber += TwitchClient_OnResubscriber;
			_credentials = new ConnectionCredentials(_twitchSettings.ChannelName, _twitchSettings.AccessToken);
			_twitchClient.Initialize(_credentials, _twitchSettings.ChannelName);


			_twitchClient.OnMessageReceived += TwitchClient_OnMessageReceived;

		}

		/// <summary>
		/// Configures the <see cref="TwitchAPI"/> instance.
		/// </summary>
		private void ConfigureTwitchAPI()
		{
			_twitchAPI = new TwitchAPI();
			_twitchAPI.Settings.ClientId = _twitchSettings.ClientId;
			_twitchAPI.Settings.AccessToken = _twitchSettings.AccessToken;
		}

		/// <summary>
		/// Configures the <see cref="LiveStreamMonitorService"/> instance.
		/// </summary>
		private void ConfigureTwitchMonitor()
		{

			_twitchMonitor = new(_twitchAPI, _twitchSettings.CheckInterval);
			_twitchMonitor.SetChannelsById(new List<string> { _twitchSettings.ChannelName });

			_twitchMonitor.OnStreamOffline += TwitchMonitor_OnStreamOffline;
			_twitchMonitor.OnStreamOnline += TwitchMonitor_OnStreamOnline;
			_twitchMonitor.OnStreamUpdate += TwitchMonitor_OnStreamUpdate;

		}

	}

}