using System;
using System.Linq;
using TaleLearnCode.TwitchCommander.Events;
using TaleLearnCode.TwitchCommander.Models;

namespace TaleLearnCode.TwitchCommander
{

	public partial class WOPR
	{

		public ProjectTracking ProjectTracking { get; private set; }
		private DateTime? _lastProjectCommandExecution;

		private void EnsureProjectIsSet()
		{
			if (ProjectTracking == null && _IsOnline)
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

		public void SetProject(string projectName)
		{
			if (_IsOnline)
			{
				// HACK: StreamId being is overridden
				ProjectTracking = ProjectTracking.Retrieve(_azureStorageSettings, _tableNames, _twitchSettings.ChannelName, projectName, _stream.Id);
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

	}

}