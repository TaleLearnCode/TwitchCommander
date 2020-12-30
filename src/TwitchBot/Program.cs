using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using TwitchBot.settings;

namespace TwitchBot
{

	class Program
	{

		private static IConfigurationRoot _config;
		private static readonly StreamLabelSettings _streamLabelSettings = new();
		private static readonly TimerIntervalSettings _timerIntervalSettings = new();
		private static readonly AzureStorageSettings _azureStorage = new();
		private static readonly TwitchSettings _twitchSettings = new();

		static void Main()
		{
			WelcomeUser();
			Initialize();

			Bot bot = new(_twitchSettings, _azureStorage, _streamLabelSettings, _timerIntervalSettings);
			bot.Connect(false);
			Console.ReadLine();
			bot.Disconnect();

		}

		static void WelcomeUser()
		{
			Console.ForegroundColor = ConsoleColor.DarkYellow;
			Console.WriteLine(@"__________        .__        __              __      __.__  __  .__      _________ .__                .___");
			Console.WriteLine(@"\______   \_______|__| ____ |  | __  ______ /  \    /  \__|/  |_|  |__   \_   ___ \|  |__ _____     __| _/");
			Console.WriteLine(@" |    |  _/\_  __ \  |/ ___\|  |/ / /  ___/ \   \/\/   /  \   __\  |  \  /    \  \/|  |  \\__  \   / __ | ");
			Console.WriteLine(@" |    |   \ |  | \/  \  \___|    <  \___ \   \        /|  ||  | |   Y  \ \     \___|   Y  \/ __ \_/ /_/ | ");
			Console.WriteLine(@" |______  / |__|  |__|\___  >__|_ \/____  >   \__/\  / |__||__| |___|  /  \______  /___|  (____  /\____ | ");
			Console.WriteLine(@"        \/                \/     \/     \/         \/                \/          \/     \/     \/      \/ ");
			Console.ForegroundColor = ConsoleColor.White;
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