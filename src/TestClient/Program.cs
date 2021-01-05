using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using TaleLearnCode.TwitchCommander;
using TaleLearnCode.TwitchCommander.AzureStorage;
using TaleLearnCode.TwitchCommander.Events;
using TaleLearnCode.TwitchCommander.Settings;

namespace TestClient
{
	class Program
	{

		private static IConfigurationRoot _config;
		private static readonly StreamLabelSettings _streamLabelSettings = new();
		private static readonly TimerIntervalSettings _timerIntervalSettings = new();
		private static readonly AzureStorageSettings _azureStorageSettings = new();
		private static readonly TwitchSettings _twitchSettings = new();

		static void Main()
		{
			Initialize();
			BotTimerSavingTest("Timer1");
			TwitchClientTesting();


			//ChatCommand chatCommand = new()
			//{
			//	ChannelName = "TaleLearnCode",
			//	Command = "testsubscribers",
			//	CommandName = "Testing Subscribers",
			//	UserPermission = UserPermission.Moderator,
			//	Response = "Be sure to check out {0} at https://twitch.tv/{0}",
			//	IsEnabledWhenStreaming = true,
			//	IsEnabledWhenNotStreaming = false,
			//	CommandResponseType = CommandResponseType.Say,
			//	UserCooldown = 0,
			//	GlobalCooldown = 0//,
			//	//CommandAliases = new List<string> { "shoutout" }
			//};
			//chatCommand.Save(_azureStorageSettings);




		}

		//private static void ChatCommandSavingTesting(string command)
		//{

		//	ChatCommand chatCommand = new()
		//	{
		//		ChannelName = "BricksWithChad",
		//		Command = command,
		//		CommandName = "Shoutout",
		//		UserPermission = UserPermission.Moderator,
		//		Response = "Be sure to check out ${0} at https://twitch.tv/${0}",
		//		IsEnabledWhenStreaming = true,
		//		IsEnabledWhenNotStreaming = false,
		//		CommandResponseType = CommandResponseType.Say,
		//		UserCooldown = 0,
		//		GlobalCooldown = 0,
		//		//CommandAliases = new List<string> { "shoutout", "shoutout2", "shoutout3", "shoutout4", "shoutout5" }
		//		CommandAliases = new List<string> { $"{command}-1", $"{command}-2", $"{command}-3", $"{command}-4", $"{command}-5" }
		//	};
		//	chatCommand.Save(_azureStorageSettings);
		//}


		private static void BotTimerSavingTest(string botTimerName)
		{
			BotTimerEntity.Save(_azureStorageSettings, "brickswithchad", botTimerName, botTimerName, 1, 1, 5);
		}

		public static void ChatCommandRetrievalTesting()
		{
			ChatCommand chatCommand = ChatCommand.RetrieveByCommand("BricksWithChad", "so", _azureStorageSettings);
			Console.WriteLine(chatCommand.CommandName);
		}

		public static void ChatCommandRetrieveListTesting()
		{

			Stopwatch stopwatch = Stopwatch.StartNew();
			List<ChatCommand> chatCommands = ChatCommand.Retrieve("BricksWithChad", _azureStorageSettings);
			Dictionary<string, ChatCommand> chatCommandDictionary = new();
			var retrievalTime = stopwatch.ElapsedMilliseconds;
			stopwatch.Restart();
			int index = 0;
			foreach (var chatCommand in chatCommands)
			{
				index++;
				Console.WriteLine(index);
				chatCommandDictionary.Add(chatCommand.Command, chatCommand);
				foreach (var commandAlias in chatCommand.CommandAliases)
				{
					chatCommandDictionary.Add(commandAlias, chatCommand);
				}
			}
			var packagingTime = stopwatch.ElapsedMilliseconds;

			Console.WriteLine($"Retrieval Time: {retrievalTime}");
			Console.WriteLine($"Packaging Time: {packagingTime}");

		}

		public static void TestChatCommandRetrieval(string command)
		{
			ChatCommand chatCommand = ChatCommand.RetrieveByCommand("BricksWithChad", command, _azureStorageSettings);
			if (chatCommand is null)
				chatCommand = ChatCommand.RetrieveByCommandAlias("BricksWithChad", command, _azureStorageSettings);

			if (chatCommand is not null)
				Console.WriteLine(chatCommand.Response);
			else
				Console.WriteLine($"Command {command} not found");
		}


		private static void TwitchClientTesting()
		{

			bool logEvents = false;

			var wopr = new WOPR(_twitchSettings, _azureStorageSettings, _timerIntervalSettings, logEvents);
			if (logEvents) wopr.OnLoggedEvent += WOPR_OnLoggedEvent;
			wopr.OnBotConnected += WOPR_OnBotConnected;
			wopr.OnBotDisconnected += WOPR_OnBotDisconnected;
			wopr.OnCommandReceived += WOPR_OnChatCommandReceived;
			wopr.OnCommandNotPermitted += WOPR_OnCommandNotPermitted;
			wopr.OnChatCommandTimedOut += WOPR_OnChatCommandTimedOut;

			wopr.OnBotTimerExecuted += WOPR_OnBotTimerExecuted;

			//wopr.Connect("BricksWithChad", logEvents);
			wopr.Start();

			Console.ReadLine();

			wopr.Disconnect();

		}

		private static void WOPR_OnBotTimerExecuted(object sender, OnBotTimerExecutedArgs e)
		{
			ConsoleHelper.PrintMessageToConsole($"[{e.ChannelName}] Executed {e.BotTimerName} bot timer : {e.ChatThresholdMet}");
		}

		private static void WOPR_OnChatCommandTimedOut(object sender, OnChatCommandTimedOutArgs e)
		{
			ConsoleHelper.PrintMessageToConsole($"[{e.LastCommandActivity.Chatter}] {e.CommandTimeoutType} time out for the \'{e.ChatCommand.CommandName}'\'.", ConsoleColor.DarkYellow);
		}

		private static void WOPR_OnCommandNotPermitted(object sender, OnCommandNotPermittedArgs e)
		{
			ConsoleHelper.PrintTextToConsole($"[{DateTime.UtcNow}]", ConsoleColor.Green);
			Console.Write(" ");
			ConsoleHelper.PrintTextToConsole($"[{e.ChatCommand.ChannelName}]", ConsoleColor.Cyan);
			ConsoleHelper.PrintTextToConsole($"Command Not Permitted for [{e.ChatMessage.Username}] {e.ChatCommand.Command}", ConsoleColor.White, ConsoleColor.Red);
			Console.Write("\n");
		}

		private static void WOPR_OnChatCommandReceived(object sender, OnCommandReceivedArgs e)
		{
			ConsoleHelper.PrintTextToConsole($"[{DateTime.UtcNow}]", ConsoleColor.Green);
			Console.Write(" ");
			ConsoleHelper.PrintTextToConsole($"[{e.ChatCommand.ChannelName}]", ConsoleColor.Cyan);
			Console.Write($" {e.ReturnedMessage}\n");
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
