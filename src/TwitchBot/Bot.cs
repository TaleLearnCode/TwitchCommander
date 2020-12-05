using System;
using System.Timers;
using TwitchBot.Commands;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Events;

namespace TwitchBot
{

	public class Bot
	{

		// TwitchTokenGenerator.com
		ConnectionCredentials credentials = new ConnectionCredentials(Settings.ChannelName, Settings.AccessToken);
		TwitchClient twitchClient = new TwitchClient();

		private Counters _counters;
		private Timer _botTimer = default;
		private int _botTimerTotal;

		internal void Connect(bool logEvents)
		{

			twitchClient.Initialize(credentials, Settings.ChannelName);

			if (logEvents)
				twitchClient.OnLog += TwitchClient_OnLog;

			twitchClient.OnConnected += TwitchClient_OnConnected;
			twitchClient.OnChatCommandReceived += TwitchClient_OnChatCommandReceived;
			twitchClient.OnDisconnected += TwitchClient_OnDisconnected;

			twitchClient.Connect();

			SetBotTimer();

			_counters = new Counters(twitchClient);

		}

		private void TwitchClient_OnDisconnected(object sender, OnDisconnectedEventArgs e)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("Bot Disconnected");
			Console.ForegroundColor = ConsoleColor.White;
		}

		internal void Disconnect()
		{
			twitchClient.Disconnect();
			if (_botTimer is not null)
				_botTimer.Dispose();
		}

		private void TwitchClient_OnLog(object sender, OnLogArgs e)
		{
			Console.WriteLine(e.Data);
		}

		private void TwitchClient_OnConnected(object sender, OnConnectedArgs e)
		{
			ConsoleHelper.PrintMessageToConsole("Bot Connected", ConsoleColor.Green, ConsoleColor.Black);
		}

		private void TwitchClient_OnChatCommandReceived(object sender, OnChatCommandReceivedArgs e)
		{

			ConsoleHelper.PrintChatMessageToConsole(e.Command.ChatMessage);

			switch (e.Command.CommandText.ToLower())
			{
				case "dropbrick":
					_counters.ExecuteDropBrick(e);
					break;
				case "oof":
					_counters.ExecuteOof(e);
					break;
				case "stats":
					_counters.ExecuteStats();
					break;
				case "setproject":
					_counters.ExecuteSetProject(e);
					break;
			}

		}

		private void SetBotTimer()
		{
			_botTimer = new Timer(Settings.TimerInternval);
			_botTimer.Elapsed += OnBotTimerElapsed;
			_botTimer.AutoReset = true;
			_botTimer.Enabled = true;
		}

		private void OnBotTimerElapsed(object sender, ElapsedEventArgs e)
		{

			ConsoleColor foregroundColor = ConsoleColor.Blue;
			ConsoleColor backgroundColor = ConsoleColor.Black;

			_botTimerTotal++;

			if (_botTimerTotal % Settings.ProjectReminderInterval == 0)
				_counters.RemindToSetProject();

			if (_botTimerTotal % Settings.WaterReminderInterval == 0)
			{
				ConsoleHelper.PrintMessageToConsole($"Water Reminder {DateTime.Now.ToLongTimeString()}", foregroundColor, backgroundColor);
				twitchClient.SendMessage(Settings.ChannelName, $"Hey {Settings.ChannelName} don't forgot to drink some water!");
			}

		}
	}

}