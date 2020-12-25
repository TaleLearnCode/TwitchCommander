namespace TaleLearnCode.TwitchCommander.Models
{

	public class ChatCommandActivity : IChatCommandActivity
	{

		public string Channel { get; set; }

		public string Chatter { get; set; }

		public string Command { get; set; }

		public double RequestTime { get; set; }

		public ChatCommandResult ChatCommandResult { get; set; }

		public string Request { get; set; }

		public string Response { get; set; }

	}

}