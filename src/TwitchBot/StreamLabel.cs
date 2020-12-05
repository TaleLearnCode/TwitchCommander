using System.IO;

namespace TwitchBot
{

	public static class StreamLabel
	{

		public static void StoreLabel(string labelName, string label)
		{
			using StreamWriter streamWriter = File.CreateText($"{Settings.StreamLabelPath}{labelName}.txt");
			streamWriter.Write(label);
		}

	}

}