using System;
using System.Linq;
using TwitchLib.Client;
using TwitchLib.Client.Events;

namespace TwitchBot.Commands
{

	public class Counters
	{

		TwitchClient _twitchClient;
		private int _bricksDropped = 0;
		private int _oofs = 0;
		private string _streamKey;
		private ProjectTracking _projectTracking = new();

		public Counters(TwitchClient twitchClient)
		{
			_twitchClient = twitchClient;
		}

		public void ExecuteDropBrick(OnChatCommandReceivedArgs commandArgs)
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

			_twitchClient.SendMessage(Settings.ChannelName, $"Chad has dropped {_bricksDropped} {(_bricksDropped == 1 ? "brick" : "bricks")} so far this stream.{(!string.IsNullOrWhiteSpace(_projectTracking.ProjectName) ? $" During the {_projectTracking.ProjectName} build, Chad has dropped {_projectTracking.BricksDropped} {(_projectTracking.BricksDropped == 1 ? "brick" : "bricks")}." : string.Empty)}");

			StreamLabel.StoreLabel("BricksDropped", $"Bricks Dropped: {_bricksDropped}");

			PrintStats();

			_projectTracking.Save();

		}

		public void ExecuteOof(OnChatCommandReceivedArgs commandArgs)
		{

			CheckForStreamId();

			if (commandArgs.Command.ChatMessage.IsBroadcaster || commandArgs.Command.ChatMessage.IsModerator)
			{
				_oofs++;
				UpdateProjectStats(oofs: 1);
			}

			_twitchClient.SendMessage(Settings.ChannelName, $"Chad has had {_oofs} {(_oofs == 1 ? "oof" : "oofs")} so far this stream.{(!string.IsNullOrWhiteSpace(_projectTracking.ProjectName) ? $" During the {_projectTracking.ProjectName} build, Chad has had {_projectTracking.Oofs} {(_projectTracking.Oofs == 1 ? "oof" : "oofs")}." : string.Empty)}");

			StreamLabel.StoreLabel("Oofs", $"Oofs: {_oofs}");

			PrintStats();

			_projectTracking.Save();

		}

		public void ExecuteStats()
		{
			CheckForStreamId();
			_twitchClient.SendMessage(Settings.ChannelName, $"Chad has dropped {_bricksDropped} {(_bricksDropped == 1 ? "brick" : "bricks")} and had {_oofs} {(_oofs == 1 ? "oof" : "oofs")} so far this stream.{(!string.IsNullOrWhiteSpace(_projectTracking.ProjectName) ? $" During the {_projectTracking.ProjectName} build, Chad has dropped {_projectTracking.BricksDropped} {(_projectTracking.BricksDropped == 1 ? "brick" : "bricks")} and had {_projectTracking.Oofs} {(_projectTracking.Oofs == 1 ? "oof" : "oofs")}." : string.Empty)}");
		}

		public void ExecuteSetProject(OnChatCommandReceivedArgs commandArgs)
		{
			CheckForStreamId();
			if (commandArgs.Command.ChatMessage.IsBroadcaster || commandArgs.Command.ChatMessage.IsModerator)
			{
				if (commandArgs.Command.ArgumentsAsList.Any())
				{
					_projectTracking = ProjectTracking.Retrieve(Settings.ChannelName, commandArgs.Command.ArgumentsAsString);
					if (_projectTracking is null)
					{
						_projectTracking = new ProjectTracking(Settings.ChannelName, commandArgs.Command.ArgumentsAsString);
						_projectTracking.Save();
					}
					_twitchClient.SendMessage(Settings.ChannelName, $"Now working on the {_projectTracking.ProjectName} project");
				}
			}
		}

		public void RemindToSetProject()
		{
			if (string.IsNullOrWhiteSpace(_projectTracking.RowKey))
			{
				_twitchClient.SendMessage(Settings.ChannelName, $"Hey {Settings.ChannelName} don't forget to set the project that you are working on.");
				// TODO: Global variable for console colors?
				ConsoleHelper.PrintMessageToConsole($"Set Project Reminder {DateTime.Now.ToLongTimeString()}", ConsoleColor.Blue, ConsoleColor.Black);
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
				var mostRecentStreamKey = StreamSnapshot.GetMostRecentStreamId("TaleLearnCode");
				if (mostRecentStreamKey != _streamKey)
				{
					_streamKey = mostRecentStreamKey;
					_bricksDropped = 0;
					_oofs = 0;
				}
			}
			catch (Exception ex)
			{
				ConsoleHelper.PrintException("GetMostRecentStreamId", ex);
			}

		}

	}

}