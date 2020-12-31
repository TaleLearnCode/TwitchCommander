using System;
using System.Collections.Generic;
using System.Timers;
using TaleLearnCode.TwitchCommander.AzureStorage;
using TaleLearnCode.TwitchCommander.Events;
using TaleLearnCode.TwitchCommander.Models;

namespace TaleLearnCode.TwitchCommander
{

	public partial class WOPR
	{

		private Timer _timer = default;

		private int TimerExecutions = 0;


		private void ConfigureTimers()
		{
			_timer = new Timer(_twitchSettings.TimerInterval);
			_timer.Elapsed += OnBotTimerElapsed;
			_timer.AutoReset = true;
			_timer.Enabled = true;
		}

		private void OnBotTimerElapsed(object sender, ElapsedEventArgs e)
		{
			TimerExecutions++;
			Console.WriteLine($"Timer Executions: {TimerExecutions}");

			List<BotTimer> botTimers = BotTimerEntity.Retrieve(_azureStorageSettings, _twitchSettings.ChannelName);
			foreach (BotTimer botTimer in botTimers)
			{
				if ((_IsOnline && botTimer.NextOnlineExecution <= DateTime.UtcNow) || (!_IsOnline && botTimer.NextOfflineExecution <= DateTime.UtcNow))
				{
					_twitchClient.SendMessage(_twitchSettings.ChannelName, botTimer.ResponseMessage);
					BotTimerEntity.BotTimerExecuted(botTimer, _azureStorageSettings);
					InvokeOnBotTimerExecuted(botTimer.BotTimerName, botTimer.ResponseMessage);
				}
			}
		}

		public EventHandler<OnBotTimerExecutedArgs> OnBotTimerExecuted;

		private void InvokeOnBotTimerExecuted(string botTimerName, string responseMessage)
		{
			OnBotTimerExecuted?.Invoke(
				this,
				new OnBotTimerExecutedArgs()
				{
					BotTimerName = botTimerName,
					ResponseMessage = responseMessage,
					ChannelName = _twitchSettings.ChannelName
				});
		}

	}

}