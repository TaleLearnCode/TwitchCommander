namespace TaleLearnCode.TwitchCommander.Settings
{

	public class TwitchSettings
	{
		public string AccessToken { get; set; }
		public string RefreshToken { get; set; }
		public string ClientId { get; set; }
		public string ChannelName { get; set; }
		public int CheckInterval { get; set; }
		public int TimerInterval { get; set; }

	}
}