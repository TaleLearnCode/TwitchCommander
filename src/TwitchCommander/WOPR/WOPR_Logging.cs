using System;
using TaleLearnCode.TwitchCommander.Events;
using TwitchLib.Client.Events;

namespace TaleLearnCode.TwitchCommander
{

	public partial class WOPR
	{

		/// <summary>
		/// Handles the <see cref="TwitchClient.OnLog"/> event.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="OnLogArgs"/> instance containing the event data.</param>
		private void TwitchClient_OnLog(object sender, OnLogArgs e)
		{
			RaiseOnLoggedEvent(e);
		}

		/// <summary>
		/// Raised when a Twitch logged event occurs.
		/// </summary>
		public EventHandler<OnLoggedEventArgs> OnLoggedEvent;

		/// <summary>
		/// Raises the <see cref="OnLoggedEvent"/> event upon Twitch logging an event.
		/// </summary>
		/// <param name="onLogArgs">The <see cref="OnLogArgs"/> arguments from the <see cref="TwitchClient.OnLog"/> event.</param>
		/// <returns></returns>
		private void RaiseOnLoggedEvent(OnLogArgs onLogArgs)
		{
			OnLoggedEvent.Invoke(this, new OnLoggedEventArgs(onLogArgs));
		}


	}

}