using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using TaleLearnCode.TwitchCommander.AzureStorage;
using TaleLearnCode.TwitchCommander.Events;
using TaleLearnCode.TwitchCommander.Extensions;
using TaleLearnCode.TwitchCommander.Models;
using TwitchLib.Client.Events;

namespace TaleLearnCode.TwitchCommander
{

	public partial class WOPR
	{

		private Timer _timer = default;
		private readonly List<ReceivedChatMessage> _receivedChatMessages = new(); // TODO: Clean up

		private TimeSpan _botRuntime = new();

		private void ConfigureTimers()
		{
			_timer = new Timer(1000);
			_timer.Elapsed += OnBotTimerElapsed;
			_timer.AutoReset = true;
			_timer.Enabled = true;
		}

		private void OnBotTimerElapsed(object sender, ElapsedEventArgs e)
		{

			TimeSpan intervalTime = new TimeSpan(0, 0, (int)(_timer.Interval / 1000));

			_botRuntime = _botRuntime.Add(intervalTime);

			if (_IsOnline && _projectTracking != null)
			//if (_projectTracking != null)
			{
				_projectTracking.ElaspedSeconds++;
				_projectTracking.OverallElapsedTime = _projectTracking.OverallElapsedTime.Add(intervalTime);

				Console.WriteLine(_projectTracking.OverallElapsedTime.ToString(@"hh\:mm\:ss"));

			}

			if (_botRuntime.TotalSeconds % 60 == 0)
			{
				List<BotTimer> botTimers = BotTimerEntity.Retrieve(_azureStorageSettings, _twitchSettings.ChannelName);
				foreach (BotTimer botTimer in botTimers)
				{
					if ((_IsOnline && botTimer.NextOnlineExecution <= DateTime.UtcNow) || (!_IsOnline && botTimer.NextOfflineExecution <= DateTime.UtcNow))
					{
						bool chatThresholdMet = _receivedChatMessages.Where(c => c.Timestamp > DateTime.UtcNow.AddMinutes(-5).ToUnixTimeSeconds()).ToList().Count >= botTimer.ChatLines;
						if (chatThresholdMet)
							_twitchClient.SendMessage(_twitchSettings.ChannelName, botTimer.ResponseMessage);
						InvokeOnBotTimerExecuted(botTimer.BotTimerName, botTimer.ResponseMessage, chatThresholdMet);
						BotTimerEntity.BotTimerExecuted(botTimer, _azureStorageSettings);
					}
				}
			}

			if (_botRuntime.TotalSeconds % (_timerIntervalSettings.ProjectReminder * 60) == 0) EnsureProjectIsSet();

		}

		public EventHandler<OnBotTimerExecutedArgs> OnBotTimerExecuted;

		private void InvokeOnBotTimerExecuted(string botTimerName, string responseMessage, bool chatThresholdMet)
		{
			OnBotTimerExecuted?.Invoke(
				this,
				new OnBotTimerExecutedArgs()
				{
					BotTimerName = botTimerName,
					ResponseMessage = responseMessage,
					ChannelName = _twitchSettings.ChannelName,
					ChatThresholdMet = chatThresholdMet
				});
		}

		private void TwitchClient_OnMessageReceived(object sender, OnMessageReceivedArgs e)
		{
			if (e.ChatMessage.Username.ToLower() != _twitchSettings.ChannelName.ToLower())
				_receivedChatMessages.Add(new ReceivedChatMessage(e.ChatMessage));
		}

	}

}