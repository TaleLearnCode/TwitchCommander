using Microsoft.Extensions.Logging;
using System;
using System.Timers;
using TaleLearnCode.TwitchCommander.Settings;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Events;

namespace TaleLearnCode.TwitchCommander
{

	public class WOPR
	{

		private static StreamLabelSettings _streamLabelSettings;
		private static TimerIntervalSettings _timerIntervalSettings;
		private static AzureStorageSettings _azureStorageSettings;
		private static TwitchSettings _twitchSettings;
		private readonly ILogger<WOPR> _logger;

		private const int _timerInterval = 1000;
		private Timer _timer = default;
		private TimeSpan _timerElapsed;

		private const int _reconnectCooldown = 30;
		private bool _stayConnected;
		private double _reconnectStart;


		public WOPR(
			TwitchSettings twitchSettings,
			AzureStorageSettings azureStorageSettings,
			StreamLabelSettings streamLabelSettings,
			TimerIntervalSettings timerIntervalSettings,
			ILogger<WOPR> logger)
		{

			_twitchSettings = twitchSettings;
			_azureStorageSettings = azureStorageSettings;
			_streamLabelSettings = streamLabelSettings;
			_timerIntervalSettings = timerIntervalSettings;
			_logger = logger;

			InitializeTimer();

		}

		ConnectionCredentials credentials;
		TwitchClient twitchClient = new();

		public void Connect(bool logEvents)
		{

			credentials = new ConnectionCredentials(_twitchSettings.ChannelName, _twitchSettings.AccessToken);

			twitchClient.Initialize(credentials, _twitchSettings.ChannelName);

			if (logEvents) twitchClient.OnLog += TwitchClient_OnLog;
			twitchClient.OnConnected += TwitchClient_OnConnected;
			twitchClient.OnChatCommandReceived += TwitchClient_OnChatCommandReceived;
			twitchClient.OnDisconnected += TwitchClient_OnDisconnected;

			twitchClient.Connect();

		}

		public void Disconnect()
		{
			_stayConnected = false;
			twitchClient.Disconnect();
			_timer?.Dispose();
		}

		private void TwitchClient_OnDisconnected(object sender, OnDisconnectedEventArgs e)
		{
			_logger.LogWarning($"{nameof(WOPR)} Disconnected");

			if (_stayConnected && (_reconnectStart == 0 || (_reconnectStart < (_timerElapsed.TotalSeconds - _reconnectCooldown))))
			{
				_stayConnected = false;
				_logger.LogInformation("Attempting to reconnect...");
				twitchClient.Connect();
				_reconnectStart = _timerElapsed.TotalSeconds;
			}
		}

		private void TwitchClient_OnChatCommandReceived(object sender, OnChatCommandReceivedArgs e)
		{
			throw new NotImplementedException();
		}

		private void TwitchClient_OnConnected(object sender, OnConnectedArgs e)
		{
			_logger.LogInformation($"{nameof(WOPR)} Connected");
		}

		private void TwitchClient_OnLog(object sender, OnLogArgs e)
		{
			_logger.LogDebug(e.Data);
		}

		private void InitializeTimer()
		{
			_timer = new Timer(_timerInterval);
			_timer.Elapsed += OnTimerElapsed;
			_timer.AutoReset = true;
			_timer.Enabled = true;
		}

		private void OnTimerElapsed(object sender, ElapsedEventArgs e)
		{
			_timerElapsed.Add(TimeSpan.FromMilliseconds(_timerInterval));
		}

	}

}