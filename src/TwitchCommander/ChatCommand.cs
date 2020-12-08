using System.Collections.Generic;

namespace TaleLearnCode.TwitchCommander
{

	public class ChatCommand
	{

		/// <summary>
		/// Gets or sets the name of the chat command.
		/// </summary>
		/// <value>
		/// A <see cref="string"/> representing the name of the chat command.
		/// </value>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the user permission.
		/// </summary>
		/// <value>
		/// A <c>string</c> representing the minimum permission level required to execute the command.
		/// </value>
		public UserPermission UserPermission { get; set; }

		public string Response { get; set; }

		public bool IsEnabledWhenStreaming { get; set; }

		public bool IsEnabledWhenNotStreaming { get; set; }

		public CommandResponseType CommandResponseType { get; set; }

		public int UserCooldown { get; set; }

		public int GlobalCooldown { get; set; }

		public List<string> CommandAliases { get; set; }

	}

}