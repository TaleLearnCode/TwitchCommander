using System;
using System.IO;

namespace TwitchBot
{

	public static class StreamLabel
	{

		public static void StoreLabel(string labelName, string label)
		{
			try
			{
				using StreamWriter streamWriter = File.CreateText($"{Settings.StreamLabelPath}{labelName}.txt");
				streamWriter.Write(label);
			}
			catch (Exception ex)
			{
				ConsoleHelper.PrintException($"StoreLabel: {labelName}", ex);
			}
		}

	}

}