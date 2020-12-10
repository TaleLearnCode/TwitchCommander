using System;
using TaleLearnCode.TwitchCommander.Events;
using TaleLearnCode.TwitchCommander.Settings;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Events;

namespace TaleLearnCode.TwitchCommander
{

	public class WOPR
	{

		private static TwitchSettings _twitchSettings;

		ConnectionCredentials credentials;
		TwitchClient twitchClient = new();

		/// <summary>
		/// Initializes a new instance of the <see cref="WOPR"/> class.
		/// </summary>
		/// <param name="twitchSettings">The twitch settings.</param>
		public WOPR(TwitchSettings twitchSettings)
		{
			_twitchSettings = twitchSettings;
		}

		#region Connect

		/// <summary>
		/// Connects to the specified Twitch channel.
		/// </summary>
		/// <param name="channelName">Name of the Twitch channel to connect to.</param>
		/// <param name="logEvents">If set to <c>true</c> then events will be logged.</param>
		/// <returns></returns>
		public void Connect(string channelName, bool logEvents)
		{

			credentials = new ConnectionCredentials(_twitchSettings.ChannelName, _twitchSettings.AccessToken);

			twitchClient.Initialize(credentials, channelName);

			if (logEvents)
				twitchClient.OnLog += TwitchClient_OnLog;

			twitchClient.OnConnected += TwitchClient_OnConnected;
			twitchClient.OnDisconnected += TwitchClient_OnDisconnected;
			twitchClient.OnChatCommandReceived += TwitchClient_OnChatCommandRecieved;

			twitchClient.Connect();

		}

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
			OnBotConnected.Invoke(this, onBotConnectedArgs);
		}

		#endregion

		#region Disconnect

		/// <summary>
		/// Disconnects the instance from Twitch.
		/// </summary>
		public void Disconnect()
		{
			twitchClient.Disconnect();
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
			OnBotDisconnected.Invoke(this, onBotDisconnectedArgs);
		}

		#endregion

		#region Logging

		/// <summary>
		/// Handles the <see cref="TwitchClient.OnLog"/> event.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="OnLogArgs"/> instance containing the event data.</param>
		/// <returns></returns>
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

		#endregion

		#region Chat Commands

		private void TwitchClient_OnChatCommandRecieved(object sender, OnChatCommandReceivedArgs e)
		{
			throw new NotImplementedException();
		}

		#endregion

	}

}