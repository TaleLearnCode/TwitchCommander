namespace TaleLearnCode.TwitchCommander.Models
{

	/// <summary>
	/// Represents the possible command response types.
	/// </summary>
	public enum CommandResponseType
	{
		/// <summary>
		/// The command will say the response in chat.
		/// </summary>
		Say,
		/// <summary>
		/// The command will reply to the requester in chat.
		/// </summary>
		Reply,
		/// <summary>
		/// The command will reply to the request in a whisper.
		/// </summary>
		Whisper
	}

}