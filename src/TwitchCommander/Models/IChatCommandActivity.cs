namespace TaleLearnCode.TwitchCommander.Models
{
	public interface IChatCommandActivity
	{
		string Channel { get; set; }
		ChatCommandResult ChatCommandResult { get; set; }
		string Chatter { get; set; }
		string Command { get; set; }
		string Request { get; set; }
		double RequestTime { get; set; }
		string Response { get; set; }
	}
}