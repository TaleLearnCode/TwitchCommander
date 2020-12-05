using System;
using TwitchLib.Client.Models;

namespace TwitchBot
{

	public static class ConsoleHelper
	{

		public static void PrintException(string operation, Exception ex)
		{
			PrintMessageToConsole($"{operation}: {ex.Message}", ConsoleColor.White, ConsoleColor.Red);
		}

		public static void PrintMessageToConsole(string message, ConsoleColor foregroundColor = ConsoleColor.White, ConsoleColor backgroundColor = ConsoleColor.Black)
		{

			ConsoleColor currentBackground = Console.BackgroundColor;
			ConsoleColor currentForeground = Console.ForegroundColor;

			Console.BackgroundColor = backgroundColor;
			Console.ForegroundColor = foregroundColor;
			Console.WriteLine(message);

			Console.ForegroundColor = currentForeground;
			Console.BackgroundColor = currentBackground;

		}

		public static void PrintChatMessageToConsole(ChatMessage chatMessage)
		{

			ConsoleColor currentBackground = Console.BackgroundColor;
			ConsoleColor currentForeground = Console.ForegroundColor;

			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.Write(chatMessage.DisplayName);
			Console.ForegroundColor = ConsoleColor.Gray;
			Console.Write($": {chatMessage.Message}\n");

			Console.ForegroundColor = currentForeground;
			Console.BackgroundColor = currentBackground;

		}



	}

}