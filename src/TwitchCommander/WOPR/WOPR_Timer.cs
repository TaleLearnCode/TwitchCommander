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

		private int elaspedMinutes = 0;

		private void ConfigureTimers()
		{
			_timer = new Timer(60000);
			_timer.Elapsed += OnBotTimerElapsed;
			_timer.AutoReset = true;
			_timer.Enabled = true;
		}

		private void OnBotTimerElapsed(object sender, ElapsedEventArgs e)
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

			elaspedMinutes++;
			if (elaspedMinutes % _timerIntervalSettings.ProjectReminder == 0) EnsureProjectIsSet();

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