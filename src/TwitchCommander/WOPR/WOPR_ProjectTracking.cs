using System;
using System.Linq;
using TaleLearnCode.TwitchCommander.Models;

namespace TaleLearnCode.TwitchCommander
{

	public partial class WOPR
	{

		private ProjectTracking _projectTracking = null;
		private DateTime? _lastProjectCommandExecution;

		private void EnsureProjectIsSet()
		{
			if (_projectTracking == null)
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
				if (_projectTracking != null)
				{
					SendMessage($"{_twitchSettings.ChannelName} is working on the '{_projectTracking.ProjectName}; project.");
				}
				else
				{
					SendMessage($"{_twitchSettings.ChannelName} has not set a project for this stream yet.");
				}
				_lastProjectCommandExecution = DateTime.UtcNow;
			}

			
		}


		private void SetProject(string projectName)
		{
			if (_IsOnline)
			{
				// HACK: StreamId being is overridden
				//_projectTracking = ProjectTracking.Retrieve(_azureStorageSettings, _twitchSettings.ChannelName, projectName, _stream.Id);
				_projectTracking = ProjectTracking.Retrieve(_azureStorageSettings, _twitchSettings.ChannelName, projectName, "TestStreamId");
				SendMessage($"{_twitchSettings.ChannelName} is now working on the '{projectName}' project.");
			}
		}

	}

}