namespace TaleLearnCode.TwitchCommander.Models
{

	public enum ChatCommandResult
	{
		Unknown = 0,
		Executed = 1,
		UserNotPermitted = 2,
		UserCooldown = 3,
		GlobalCooldown = 4
	}

}