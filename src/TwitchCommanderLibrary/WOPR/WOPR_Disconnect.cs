using System;
using TaleLearnCode.TwitchCommander.Events;
using TwitchLib.Communication.Events;

namespace TaleLearnCode.TwitchCommander
{
	public partial class WOPR
	{

		/// <summary>
		/// Disconnects the instance from Twitch.
		/// </summary>
		public void Disconnect()
		{
			_twitchClient.Disconnect();
		}

		/// <summary>
		/// Handles the <see cref="TwitchClient.OnDisconnected"/> event.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="OnDisconnectedEventArgs"/> instance containing the event data.</param>
		private void TwitchClient_OnDisconnected(object sender, OnDisconnectedEventArgs e)
		{
			RaiseOnBotDisconnected();
		}

		/// <summary>
		/// Raised when the bot disconnected from Twitch.
		/// </summary>
		public EventHandler<OnBotDisconnectedArgs> OnBotDisconnected;

		/// <summary>
		/// Raises the <see cref="OnBotDisconnected"/> event upon the bot disconnecting from Twitch.
		/// </summary>
		private void RaiseOnBotDisconnected()
		{
			OnBotDisconnectedArgs onBotDisconnectedArgs = new(_twitchSettings.ChannelName);
			OnBotDisconnected?.Invoke(this, onBotDisconnectedArgs);
		}

	}

}