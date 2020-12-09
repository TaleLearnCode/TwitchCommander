using Microsoft.Extensions.Configuration;
using System;
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
		private static AzureStorageSettings _azureStorage = new();
		private static TwitchSettings _twitchSettings = new();

		static void Main(string[] args)
		{
			Initialize();

			//WOPR wopr = new WOPR(_twitchSettings, _azureStorage, _streamLabelSettings, _timerIntervalSettings);
			//wopr.Connect(false);
			//Console.ReadLine();
			//wopr.Disconnect();

			bool logEvents = true;


			var woprTest = new WOPR(_twitchSettings);
			if (logEvents) woprTest.OnLoggedEvent += WOPRTest_OnLoggedEvent;
			woprTest.OnBotConnected += WOPRTest_OnBotConnected;
			woprTest.OnBotDisconnected += WOPRTest_OnBotDisconnected;

			woprTest.Connect("BricksWithChad", logEvents);

			Console.ReadLine();

			woprTest.Disconnect();

		}

		private static void WOPRTest_OnLoggedEvent(object sender, OnLoggedEventArgs e)
		{
			ConsoleHelper.PrintTextToConsole($"[{e.DateTime}]", ConsoleColor.Blue);
			Console.Write(" ");
			ConsoleHelper.PrintTextToConsole(e.BotUsername, ConsoleColor.White, ConsoleColor.Magenta);
			Console.Write(" ");
			ConsoleHelper.PrintTextToConsole(e.Data, ConsoleColor.Yellow);
			Console.Write("\n");

		}

		private static void WOPRTest_OnBotDisconnected(object sender, OnBotDisconnectedArgs e)
		{
			Console.WriteLine($"{e.BotUsername} has disconnected");
		}

		private static void WOPRTest_OnBotConnected(object sender, OnBotConnectedArgs e)
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
			_config.GetSection("AzureStorage").Bind(_azureStorage);
			_config.GetSection("Twitch").Bind(_twitchSettings);

		}

	}
}
