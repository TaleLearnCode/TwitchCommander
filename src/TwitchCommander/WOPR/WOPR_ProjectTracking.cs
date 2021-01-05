using TaleLearnCode.TwitchCommander.Models;

namespace TaleLearnCode.TwitchCommander
{

	public partial class WOPR
	{

		private ProjectTracking _projectTracking = null;

		private void EnsureProjectIsSet()
		{
			if (_projectTracking == null)
				SendMessage($"Hey @{_twitchSettings.ChannelName}, don't forget to set the project you are working on.");
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