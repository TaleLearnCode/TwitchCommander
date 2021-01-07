using System;

namespace TaleLearnCode.TwitchCommander.Exceptions
{
	public class SettingMissingException : Exception
	{

		public string Setting { get; set; }

		public SettingMissingException(string setting) : base($"Missing the value for the '{setting}' setting.")
		{
			Setting = setting;
		}

	}

}