using System.Collections.Generic;
using TaleLearnCode.TwitchCommander.Models;
using TwitchLib.Api;
using TwitchLib.Api.Services;
using TwitchLib.Client;
using TwitchLib.Client.Models;

namespace TaleLearnCode.TwitchCommander
{

	public partial class WOPR
	{

		private readonly AppSettings _appSettings;
		private readonly AzureStorageSettings _azureStorageSettings;
		private readonly TableNames _tableNames;
		private readonly TwitchSettings _twitchSettings;

		private readonly TwitchClient _twitchClient = new();
		private ConnectionCredentials _credentials;
		private TwitchAPI _twitchAPI;
		private LiveStreamMonitorService _twitchMonitor;

		private bool _IsOnline;

		public string ChannelName { get; }

		/// <summary>
		/// Initializes a new instance of the <see cref="WOPR"/> class.
		/// </summary>
		/// <param name="appSettings">The application settings.</param>
		/// <param name="azureStorageSetttings">The azure storage setttings.</param>
		/// <param name="tableNames">The table names.</param>
		/// <param name="twitchSettings">The Twitch settings.</param>
		/// <param name="viewLogs"></param>
		public WOPR(
			string channelName,
			AppSettings appSettings,
			AzureStorageSettings azureStorageSetttings,
			TableNames tableNames,
			TwitchSettings twitchSettings,
			bool viewLogs)
		{

			ChannelName = channelName;
			_appSettings = appSettings;
			_azureStorageSettings = azureStorageSetttings;
			_tableNames = tableNames;
			_twitchSettings = twitchSettings;

			ConfigureTwitchClient(viewLogs);
			ConfigureTwitchAPI();
			ConfigureTwitchMonitor();

		}

		/// <summary>
		/// Starts monitoring Twitch in order to perform the necessary actions.
		/// </summary>
		public void Connect()
		{
			_twitchClient.Connect();
			_twitchMonitor.Start();
			ConfigureTimers();
		}

		/// <summary>
		/// Configures the <see cref="TwitchClient"/> instance.
		/// </summary>
		/// <param name="viewLogs">If set to <c>true</c> then the logs will be sent to the consumer.</param>
		private void ConfigureTwitchClient(bool viewLogs)
		{
			if (viewLogs) _twitchClient.OnLog += TwitchClient_OnLog;
			_twitchClient.OnConnected += TwitchClient_OnConnected;
			_twitchClient.OnDisconnected += TwitchClient_OnDisconnected;
			_twitchClient.OnChatCommandReceived += TwitchClient_OnChatCommandReceived;
			_twitchClient.OnNewSubscriber += TwitchClient_OnNewSubscriber;
			_twitchClient.OnCommunitySubscription += TwitchClient_OnCommunitySubscription;
			_twitchClient.OnGiftedSubscription += TwitchClient_OnGiftedSubscription;
			_twitchClient.OnReSubscriber += TwitchClient_OnResubscriber;
			_twitchClient.OnMessageReceived += TwitchClient_OnMessageReceived;

			_credentials = new ConnectionCredentials(_twitchSettings.BotName, _twitchSettings.AccessToken);
			_twitchClient.Initialize(_credentials, _twitchSettings.ChannelName);
		}

		/// <summary>
		/// Configures the <see cref="TwitchAPI"/> instance.
		/// </summary>
		private void ConfigureTwitchAPI()
		{
			_twitchAPI = new();
			_twitchAPI.Settings.ClientId = _twitchSettings.ClientId;
			_twitchAPI.Settings.AccessToken = _twitchSettings.AccessToken;
		}

		/// <summary>
		/// Configures the <see cref="LiveStreamMonitorService"/> instance.
		/// </summary>
		private void ConfigureTwitchMonitor()
		{

			_twitchMonitor = new(_twitchAPI, _appSettings.StreamMonitorCheckInterval);
			_twitchMonitor.SetChannelsById(new List<string> { _twitchSettings.ChannelName });

		}

		/// <summary>
		/// Sends the message to the Twitch channel.
		/// </summary>
		/// <param name="message">The message to be sent to the Twitch channel's chat..</param>
		/// <returns></returns>
		private void SendMessage(string message)
		{
			_twitchClient.SendMessage(_twitchSettings.ChannelName, message);
		}

	}

}