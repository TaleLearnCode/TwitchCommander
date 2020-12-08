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

		private LegoStats _legoStats;
		private Timer _botTimer = default;
		private int _botTimerTotal;
		private bool _stayConnected;
		private int _reconnectStart;

		private const int _timerInternval = 1000;
		private const int _reconnectCooldown = 30;


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

			_legoStats = new LegoStats(twitchClient);

			_stayConnected = true;

		}

		internal void Disconnect()
		{
			_stayConnected = false;
			twitchClient.Disconnect();
			if (_botTimer is not null)
				_botTimer.Dispose();
		}

		private void TwitchClient_OnDisconnected(object sender, OnDisconnectedEventArgs e)
		{

			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("Bot Disconnected");
			Console.ForegroundColor = ConsoleColor.White;

			if (_stayConnected && (_reconnectStart == 0 || (_reconnectStart < (_botTimerTotal - _reconnectCooldown))))
			{
				_stayConnected = false;
				Console.BackgroundColor = ConsoleColor.Red;
				Console.WriteLine("Attempting to reconnect...");
				Console.BackgroundColor = ConsoleColor.Black;
				twitchClient.Connect();
				_reconnectStart = _botTimerTotal;
			}
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
					_legoStats.DropBrick(e);
					break;
				case "oof":
					_legoStats.Oof(e);
					break;
				case "stats":
					_legoStats.Stats();
					break;
				case "setproject":
					_legoStats.SetProject(e);
					break;
				case "project":
					_legoStats.Project();
					break;
			}

		}

		private void SetBotTimer()
		{
			_botTimer = new Timer(_timerInternval);
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
				_legoStats.RemindToSetProject();

			if (_botTimerTotal % Settings.WaterReminderInterval == 0)
			{
				ConsoleHelper.PrintMessageToConsole($"Water Reminder {DateTime.Now.ToLongTimeString()}", foregroundColor, backgroundColor);
				twitchClient.SendMessage(Settings.ChannelName, $"Hey {Settings.ChannelName} don't forgot to drink some water!");
			}

			_legoStats.UpdateProjectTimer();

		}
	}

}