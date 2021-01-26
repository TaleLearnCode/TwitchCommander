using System;
using System.Linq;
using TaleLearnCode.TwitchCommander.AzureStorage;
using TaleLearnCode.TwitchCommander.Events;
using TaleLearnCode.TwitchCommander.Models;
using TwitchLib.Client.Models;

namespace TaleLearnCode.TwitchCommander
{

	public partial class WOPR
	{

		public ProjectTracking ProjectTracking { get; private set; }
		private DateTime? _lastProjectCommandExecution;

		private void EnsureProjectIsSet()
		{
			if (ProjectTracking == null && (_IsOnline || _fakeOnline))
				SendMessage($"Hey @{_twitchSettings.ChannelName}, don't forget to set the project you are working on.");
		}

		private void HandleProjectCommand(TwitchLib.Client.Models.ChatCommand chatCommand)
		{
			if (chatCommand.ArgumentsAsList.Any() && UserPermittedToExecuteCommand(UserPermission.Moderator, chatCommand.ChatMessage))
			{
				SetProject(chatCommand.ArgumentsAsString);
			}
			else if (_lastProjectCommandExecution == null || (DateTime)_lastProjectCommandExecution <= DateTime.UtcNow.AddMinutes(-1))
			{
				if (ProjectTracking != null)
				{
					SendMessage($"{_twitchSettings.ChannelName} is working on the '{ProjectTracking.ProjectName}; project.");
				}
				else
				{
					SendMessage($"{_twitchSettings.ChannelName} has not set a project for this stream yet.");
				}
				_lastProjectCommandExecution = DateTime.UtcNow;
			}
		}

		private void HandleBrickDropCommand(ChatCommand chatCommand)
		{
			if (ProjectTracking != null)
			{
				if (UserPermittedToExecuteCommand(UserPermission.Broadcaster, chatCommand.ChatMessage))
				{
					int bricksDropped = 1;
					if (chatCommand.ArgumentsAsList.Any()) int.TryParse(chatCommand.ArgumentsAsString, out bricksDropped);
					ProjectTracking.DroppedBricks += bricksDropped;
					ProjectTracking.OverallDroppedBricks += bricksDropped;
					ProjectTrackingEntity.Save(_azureStorageSettings, _tableNames, ProjectTracking);
					SendMessage($"{_twitchSettings.ChannelName} just dropped {bricksDropped} bricks.  That's {ProjectTracking.DroppedBricks} so far this stream and {ProjectTracking.OverallDroppedBricks} while working on the {ProjectTracking.ProjectName} project.");
					OnBrickDropped?.Invoke(this, new OnBrickDropArgs(ProjectTracking.DroppedBricks, ProjectTracking.OverallDroppedBricks));
				}
				else
				{
					SendMessage($"{_twitchSettings.ChannelName} has dropped {ProjectTracking.DroppedBricks} this stream.  Over the course of the {ProjectTracking.ProjectName} project, {ProjectTracking.ChannelName} has dropped {ProjectTracking.OverallDroppedBricks} bricks.");
				}
			}
			else
			{
				if (UserPermittedToExecuteCommand(UserPermission.Broadcaster, chatCommand.ChatMessage))
					SendMessage($"Hey @{_twitchSettings.ChannelName}, you need set a project first.");
				else
					SendMessage($"{_twitchSettings.ChannelName} needs to set a project first.");
			}
		}

		private void HandleOofCommand(ChatCommand chatCommand)
		{
			if (ProjectTracking != null)
			{
				if (UserPermittedToExecuteCommand(UserPermission.Broadcaster, chatCommand.ChatMessage))
				{
					ProjectTracking.Oofs++;
					ProjectTracking.OverallOofs++;
					ProjectTrackingEntity.Save(_azureStorageSettings, _tableNames, ProjectTracking);
					SendMessage($"{_twitchSettings.ChannelName} just oofed.  That's {ProjectTracking.Oofs} oofs this stream and {ProjectTracking.OverallOofs} oofs while working on the {ProjectTracking.ProjectName} project.");
					OnOof?.Invoke(this, new OnOofArgs(ProjectTracking.Oofs, ProjectTracking.OverallOofs));
				}
				else
				{
					SendMessage($"{_twitchSettings.ChannelName} has had {ProjectTracking.Oofs} oofs the stream.  That's {ProjectTracking.OverallOofs} oofs so far on the {ProjectTracking.ProjectName} project.");
				}
			}
			else
			{
				if (UserPermittedToExecuteCommand(UserPermission.Broadcaster, chatCommand.ChatMessage))
					SendMessage($"Hey @{_twitchSettings.ChannelName}, you need set a project first.");
				else
					SendMessage($"{_twitchSettings.ChannelName} needs to set a project first.");
			}
		}

		public void SetProject(string projectName)
		{
			if (_IsOnline || _fakeOnline)
			{
				// HACK: StreamId being is overridden
				ProjectTracking = ProjectTracking.Retrieve(_azureStorageSettings, _tableNames, _twitchSettings.ChannelName, projectName, (_fakeOnline) ? "TestStreamId" : _stream.Id);
				//_projectTracking = ProjectTracking.Retrieve(_azureStorageSettings, _tableNames, _twitchSettings.ChannelName, projectName, "TestStreamId");
				SendMessage($"{_twitchSettings.ChannelName} is now working on the '{projectName}' project.");
				InvokeOnProjectUpdated();
			}
		}

		/// <summary>
		/// Raised when an element of the <see cref="Models.ProjectTracking"/> is updated.
		/// </summary>
		/// <remarks>Not thrown when the thee project timer is updated.</remarks>
		public EventHandler<OnProjectUpdatedArgs> OnProjectUpdated;

		/// <summary>
		/// Invokes the <see cref="OnProjectUpdated"/> event.
		/// </summary>
		private void InvokeOnProjectUpdated()
		{
			OnProjectUpdated?.Invoke(this, new(ProjectTracking));
		}

		public EventHandler<OnBrickDropArgs> OnBrickDropped;

		public EventHandler<OnOofArgs> OnOof;

	}

}