using System;
using TaleLearnCode.TwitchCommander.Events;
using TwitchLib.Client.Events;

namespace TaleLearnCode.TwitchCommander
{
	public partial class WOPR
	{

		/// <summary>
		/// Handles the <see cref="TwitchClient.OnConnected"/> event.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="OnConnectedArgs"/> instance containing the event data.</param>
		private void TwitchClient_OnConnected(object sender, OnConnectedArgs e)
		{
			RaiseOnBotConnected(e);
		}

		/// <summary>
		/// Raised when the bot connects to Twitch.
		/// </summary>
		public EventHandler<OnBotConnectedArgs> OnBotConnected;

		/// <summary>
		/// Raises the <see cref="OnBotConnected"/> event upon the bot connecting to Twitch.
		/// </summary>
		/// <param name="onConnectedArgs">The <see cref="OnConnectedArgs"/> arguments from the <see cref="TwitchClient.OnConnected"/> event.</param>
		/// <returns></returns>
		private void RaiseOnBotConnected(OnConnectedArgs onConnectedArgs)
		{
			OnBotConnectedArgs onBotConnectedArgs = new(onConnectedArgs);
			OnBotConnected?.Invoke(this, onBotConnectedArgs);
		}

	}

}