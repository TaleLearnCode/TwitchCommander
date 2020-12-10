using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using TaleLearnCode.TwitchCommander;
using TaleLearnCode.TwitchCommander.Events;
using TaleLearnCode.TwitchCommander.Settings;

namespace TestClient
{
	class Program
	{

		private static IConfigurationRoot _config;
		private static StreamLabelSettings _streamLabelSettings = new();
		private static TimerIntervalSettings _timerIntervalSettings = new();
		private static AzureStorageSettings _azureStorageSettings = new();
		private static TwitchSettings _twitchSettings = new();

		static void Main(string[] args)
		{
			Initialize();
			//TwitchClientTesting();
			ChatCommandSavingTesting();
			Console.ReadLine();
			ChatCommandRetrievalTesting();

		}

		private static void ChatCommandSavingTesting()
		{
			ChatCommand chatCommand = new()
			{
				ChannelName = "BricksWithChad",
				Command = "so",
				CommandName = "Shoutout",
				UserPermission = UserPermission.Moderator,
				Response = "Be sure to check out ${0} at https://twitch.tv/${0}",
				IsEnabledWhenStreaming = true,
				IsEnabledWhenNotStreaming = false,
				CommandResponseType = CommandResponseType.Say,
				UserCooldown = 0,
				GlobalCooldown = 0,
				CommandAliases = new List<string> { "so", "shoutout" }
			};
			chatCommand.Save(_azureStorageSettings);
		}

		public static void ChatCommandRetrievalTesting()
		{
			ChatCommand chatCommand = ChatCommand.Retrieve("BricksWithChad", "so", _azureStorageSettings);
			Console.WriteLine(chatCommand.CommandName);
		}

		private static void TwitchClientTesting()
		{

			bool logEvents = true;

			var wopr = new WOPR(_twitchSettings);
			if (logEvents) wopr.OnLoggedEvent += WOPR_OnLoggedEvent;
			wopr.OnBotConnected += WOPR_OnBotConnected;
			wopr.OnBotDisconnected += WOPR_OnBotDisconnected;

			wopr.Connect("BricksWithChad", logEvents);

			Console.ReadLine();

			wopr.Disconnect();

		}

		private static void WOPR_OnLoggedEvent(object sender, OnLoggedEventArgs e)
		{
			ConsoleHelper.PrintTextToConsole($"[{e.DateTime}]", ConsoleColor.Blue);
			Console.Write(" ");
			ConsoleHelper.PrintTextToConsole(e.BotUsername, ConsoleColor.White, ConsoleColor.Magenta);
			Console.Write(" ");
			ConsoleHelper.PrintTextToConsole(e.Data, ConsoleColor.Yellow);
			Console.Write("\n");

		}

		private static void WOPR_OnBotDisconnected(object sender, OnBotDisconnectedArgs e)
		{
			Console.WriteLine($"{e.BotUsername} has disconnected");
		}

		private static void WOPR_OnBotConnected(object sender, OnBotConnectedArgs e)
		{
			Console.WriteLine($"{e.BotUsername} is connected to {e.AutoJoinChannel}");
		}

		private static void Initialize()
		{
			var builder = new ConfigurationBuilder()
				.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "Settings"))
				.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
				.AddJsonFile("AzureStorage.json", optional: true, reloadOnChange: true)
				.AddJsonFile("TimerIntervals.json", optional: true, reloadOnChange: true)
				.AddJsonFile("Twitch.json", optional: true, reloadOnChange: true);
			_config = builder.Build();

			_config.GetSection("StreamLabels").Bind(_streamLabelSettings);
			_config.GetSection("TimerInterval").Bind(_timerIntervalSettings);
			_config.GetSection("AzureStorage").Bind(_azureStorageSettings);
			_config.GetSection("Twitch").Bind(_twitchSettings);

		}

	}
}
