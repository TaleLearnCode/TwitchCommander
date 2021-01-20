using System;

namespace TaleLearnCode.TwitchCommander.Events
{

	/// <summary>
	/// Arguments returned when an oof occurs.
	/// </summary>
	/// <seealso cref="System.EventArgs" />
	public class OnOofArgs : EventArgs
	{

		/// <summary>
		/// Gets the number of oofs during the stream.
		/// </summary>
		/// <value>
		/// A <c>int</c> representing the number of oofs during the stream.
		/// </value>
		public int OofsDuringStream { get; init; }

		/// <summary>
		/// Gets the number of oofs during the project.
		/// </summary>
		/// <value>
		/// A <c>int</c> representing the number of oofs during the project.
		/// </value>
		public int OofsDuringProject { get; init; }

		public OnOofArgs() { }

		public OnOofArgs(int oofsDuringStream, int oofsDuringProject)
		{
			OofsDuringStream = oofsDuringStream;
			OofsDuringProject = oofsDuringProject;
		}

	}

}