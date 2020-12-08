using System;
using System.Linq;
using TwitchBot.settings;
using TwitchLib.Client;
using TwitchLib.Client.Events;

namespace TwitchBot.Commands
{

	public class LegoStats
	{

		TwitchClient _twitchClient;
		private int _bricksDropped = 0;
		private int _oofs = 0;
		private string _streamKey;
		private ProjectTracking _projectTracking = new();

		private static TwitchSettings _twitchSettings;
		private static StreamLabelSettings _streamLabelSettings;
		private static AzureStorageSettings _azureStorageSettings;

		public LegoStats(TwitchClient twitchClient, TwitchSettings twitchSettings, StreamLabelSettings streamLabelSettings, AzureStorageSettings azureStorageSettings)
		{
			_twitchClient = twitchClient;
			_twitchSettings = twitchSettings;
			_streamLabelSettings = streamLabelSettings;
			_azureStorageSettings = azureStorageSettings;
		}

		public void DropBrick(OnChatCommandReceivedArgs commandArgs)
		{

			CheckForStreamId();

			if (commandArgs.Command.ChatMessage.IsBroadcaster || commandArgs.Command.ChatMessage.IsModerator)
			{
				int numberOfBricksDropped = 1;
				if (commandArgs.Command.ArgumentsAsList.Any())
					int.TryParse(commandArgs.Command.ArgumentsAsList[0], out numberOfBricksDropped);

				_bricksDropped += numberOfBricksDropped;
				UpdateProjectStats(bricksDropped: numberOfBricksDropped);
			}

			_twitchClient.SendMessage(_twitchSettings.ChannelName, $"Chad has dropped {_bricksDropped} {(_bricksDropped == 1 ? "brick" : "bricks")} so far this stream.{(!string.IsNullOrWhiteSpace(_projectTracking.ProjectName) ? $" During the {_projectTracking.ProjectName} build, Chad has dropped {_projectTracking.BricksDropped} {(_projectTracking.BricksDropped == 1 ? "brick" : "bricks")}." : string.Empty)}");

			WriteStreamDropLabel();
			WriteProjectDropLabel();

			PrintStats();

			_projectTracking.Save();

		}

		public void Oof(OnChatCommandReceivedArgs commandArgs)
		{

			CheckForStreamId();

			if (commandArgs.Command.ChatMessage.IsBroadcaster || commandArgs.Command.ChatMessage.IsModerator)
			{
				_oofs++;
				UpdateProjectStats(oofs: 1);
			}

			_twitchClient.SendMessage(_twitchSettings.ChannelName, $"Chad has had {_oofs} {(_oofs == 1 ? "oof" : "oofs")} so far this stream.{(!string.IsNullOrWhiteSpace(_projectTracking.ProjectName) ? $" During the {_projectTracking.ProjectName} build, Chad has had {_projectTracking.Oofs} {(_projectTracking.Oofs == 1 ? "oof" : "oofs")}." : string.Empty)}");

			WriteStreamOofLabel();
			WriteProjectOofLabel();

			PrintStats();

			_projectTracking.Save();

		}

		public void Stats()
		{
			CheckForStreamId();
			_twitchClient.SendMessage(_twitchSettings.ChannelName, $"Chad has dropped {_bricksDropped} {(_bricksDropped == 1 ? "brick" : "bricks")} and had {_oofs} {(_oofs == 1 ? "oof" : "oofs")} so far this stream.{(!string.IsNullOrWhiteSpace(_projectTracking.ProjectName) ? $" During the {_projectTracking.ProjectName} build, Chad has dropped {_projectTracking.BricksDropped} {(_projectTracking.BricksDropped == 1 ? "brick" : "bricks")} and had {_projectTracking.Oofs} {(_projectTracking.Oofs == 1 ? "oof" : "oofs")}." : string.Empty)}");
		}

		public void SetProject(OnChatCommandReceivedArgs commandArgs)
		{
			CheckForStreamId();
			if (commandArgs.Command.ChatMessage.IsBroadcaster || commandArgs.Command.ChatMessage.IsModerator)
			{
				if (commandArgs.Command.ArgumentsAsList.Any())
				{
					_projectTracking = ProjectTracking.Retrieve(_twitchSettings.ChannelName, commandArgs.Command.ArgumentsAsString, _azureStorageSettings);
					if (_projectTracking is null)
					{
						_projectTracking = new ProjectTracking(_twitchSettings.ChannelName, commandArgs.Command.ArgumentsAsString, _azureStorageSettings);
						_projectTracking.Save();
					}
					_twitchClient.SendMessage(_twitchSettings.ChannelName, $"Now working on the {_projectTracking.ProjectName} project");
					WriteProjectDropLabel();
					WriteProjectOofLabel();
					WriteProjectTimerLabel();
					StreamLabel.StoreLabel(_streamLabelSettings.OutputPath, "Project", _projectTracking.ProjectName);
				}
			}
		}

		public void Project()
		{
			CheckForStreamId();
			_twitchClient.SendMessage(_twitchSettings.ChannelName, $"Chad is currently building the {_projectTracking.ProjectName}.");
		}

		public void RemindToSetProject()
		{
			if (string.IsNullOrWhiteSpace(_projectTracking.RowKey))
			{
				_twitchClient.SendMessage(_twitchSettings.ChannelName, $"Hey {_twitchSettings.ChannelName} don't forget to set the project that you are working on.");
				// TODO: Global variable for console colors?
				ConsoleHelper.PrintMessageToConsole($"Set Project Reminder {DateTime.Now.ToLongTimeString()}", ConsoleColor.Blue, ConsoleColor.Black);
			}

		}

		public void UpdateProjectTimer()
		{
			if (!string.IsNullOrWhiteSpace(_projectTracking.RowKey))
			{
				_projectTracking.ProjectTimer++;
				WriteProjectTimerLabel();
			}
		}

		private void UpdateProjectStats(int bricksDropped = 0, int oofs = 0)
		{
			_projectTracking.BricksDropped += bricksDropped;
			_projectTracking.Oofs += oofs;
		}

		private void PrintStats()
		{
			Console.WriteLine($"\tBricks Dropped: {_bricksDropped}");
			Console.WriteLine($"\tOofs: {_oofs}");
		}

		private void CheckForStreamId()
		{
			try
			{
				// TODO: Remove the hard-coded UserName
				var mostRecentStreamKey = StreamSnapshot.GetMostRecentStreamId("TaleLearnCode", _azureStorageSettings);
				if (mostRecentStreamKey != _streamKey)
				{
					_streamKey = mostRecentStreamKey;
					_bricksDropped = 0;
					_oofs = 0;
					WriteStreamDropLabel();
					WriteStreamOofLabel();
				}
			}
			catch (Exception ex)
			{
				ConsoleHelper.PrintException("GetMostRecentStreamId", ex);
			}

		}

		private void WriteProjectDropLabel()
		{
			StreamLabel.StoreLabel(_streamLabelSettings.OutputPath, "BricksDropped-Project", $"Bricks Dropped: {_projectTracking.BricksDropped}");
		}

		private void WriteProjectOofLabel()
		{
			StreamLabel.StoreLabel(_streamLabelSettings.OutputPath, "Oofs-Project", $"Oofs: {_projectTracking.Oofs}");
		}

		private void WriteProjectTimerLabel()
		{
			StreamLabel.StoreLabel(_streamLabelSettings.OutputPath, "ProjectTimer", (new TimeSpan(0, 0, _projectTracking.ProjectTimer).ToString(@"hh\:mm\:ss")));
		}

		private void WriteStreamDropLabel()
		{
			StreamLabel.StoreLabel(_streamLabelSettings.OutputPath, "BricksDropped-Stream", $"Bricks Dropped: {_bricksDropped}");
		}

		private void WriteStreamOofLabel()
		{
			StreamLabel.StoreLabel(_streamLabelSettings.OutputPath, "Oofs-Stream", $"Oofs: {_oofs}");
		}

	}

}