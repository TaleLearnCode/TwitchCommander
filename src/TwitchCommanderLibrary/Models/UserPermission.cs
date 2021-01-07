namespace TaleLearnCode.TwitchCommander.Models
{

	/// <summary>
	/// Represents the different user permission types.
	/// </summary>
	public enum UserPermission
	{
		/// <summary>
		/// Everyone has permission to perform a task.
		/// </summary>
		Everyone,
		/// <summary>
		/// Only subscribers or above have permission to perform a task.
		/// </summary>
		Subscriber,
		/// <summary>
		/// Only VIPs or above have permission to perform a task.
		/// </summary>
		VIP,
		/// <summary>
		/// Only moderators or above have permission to perform a task.
		/// </summary>
		Moderator,
		/// <summary>
		/// Only broadcasters have permission to perform a task.
		/// </summary>
		Broadcaster
	}

}