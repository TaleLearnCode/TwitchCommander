namespace TaleLearnCode.TwitchCommander.Models
{
	public interface IProjectTracking
	{

		/// <summary>
		/// Gets or sets the name of the channel.
		/// </summary>
		/// <value>
		/// A <c>string</c> representing the channel name.
		/// </value>
		string ChannelName { get; set; }

		/// <summary>
		/// Gets or sets the name of the project being tracked.
		/// </summary>
		/// <value>
		/// The name of the project being tracked.
		/// </value>
		string ProjectName { get; set; }

		/// <summary>
		/// Gets or sets the stream identifier.
		/// </summary>
		/// <value>
		/// The stream identifier.
		/// </value>
		string StreamId { get; set; }

		/// <summary>
		/// Gets or sets the details of the project being tracked.
		/// </summary>
		/// <value>
		/// The details of the project being tracked.
		/// </value>
		string Details { get; set; }

		/// <summary>
		/// Gets or sets the elapsed number of seconds while working on the project.
		/// </summary>
		/// <value>
		/// The elapsed number of seconds while working on the project.
		/// </value>
		int ElaspedSeconds { get; set; }

		/// <summary>
		/// Gets or sets the number of bricks dropped while working on the project.
		/// </summary>
		/// <value>
		/// The number of bricks dropped while working on the project.
		/// </value>
		int DroppedBricks { get; set; }

		/// <summary>
		/// Gets or sets the number of oofs (mistakes) while working on the project.
		/// </summary>
		/// <value>
		/// The number of oofs.
		/// </value>
		int Oofs { get; set; }

	}
}