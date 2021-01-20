using System;

namespace TaleLearnCode.TwitchCommander.Events
{

	/// <summary>
	/// Arguments returned when a bricked is dropped.
	/// </summary>
	public class OnBrickDropArgs : EventArgs
	{

		/// <summary>
		/// Gets the number of bricks dropped during the stream.
		/// </summary>
		/// <value>
		/// A <c>int</c> representing the number of bricks dropped during the stream.
		/// </value>
		public int BricksDroppedDuringStream { get; init; }

		/// <summary>
		/// Gets the number bricks dropped during project.
		/// </summary>
		/// <value>
		/// A <c>int</c> representing tthe number of bricks dropped during the project.
		/// </value>
		public int BricksDroppedDuringProject { get; init; }

		public OnBrickDropArgs() { }

		public OnBrickDropArgs(int bricksDroppedDuringStream, int bricksDroppedDuringProject)
		{
			BricksDroppedDuringStream = bricksDroppedDuringStream;
			BricksDroppedDuringProject = bricksDroppedDuringProject;
		}

	}

}