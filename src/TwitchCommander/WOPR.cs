using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaleLearnCode.TwitchCommander.Events;
using TaleLearnCode.TwitchCommander.Settings;
using TwitchLib.Api;
using TwitchLib.Api.Services;
using TwitchLib.Api.Services.Events.LiveStreamMonitor;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Events;

namespace TaleLearnCode.TwitchCommander
{

	public class WOPR
	{

		private readonly TwitchSettings _twitchSettings;
		private readonly AzureStorageSettings _azureStorageSettings;

		private ConnectionCredentials credentials;
		private TwitchClient _twitchClient = new();

		private TwitchAPI _twitchAPI;
		private LiveStreamMonitorService _twitchMonitor;

		private bool _IsOnline;

		/// <summary>
		/// Initializes a new instance of the <see cref="WOPR"/> class.
		/// </summary>
		/// <param name="twitchSettings">The twitch settings.</param>
		public WOPR(
			TwitchSettings twitchSettings,
			AzureStorageSettings azureStorageSettings,
			bool logEvents)
		{
			_twitchSettings = twitchSettings;
			_azureStorageSettings = azureStorageSettings;
		}

		public async Task StartAsync(bool logEvents)
		{
			ConnectTwitchClient(logEvents);
			ConnectTwitchAPI();
			ConfigureLiveMonitor();
		}

		#region TwitchMonitor

		private void ConfigureLiveMonitor()
		{

			// TODO Make the CheckInterval a setting
			_twitchMonitor = new(_twitchAPI, 5);

			_twitchMonitor.SetChannelsById(new List<string> { _twitchSettings.ChannelName });

			_twitchMonitor.OnStreamOffline += TwitchMonitor_OnStreamOffline;
			_twitchMonitor.OnStreamOnline += TwitchMonitor_OnStreamOnline;
			_twitchMonitor.OnStreamUpdate += TwitchMonitor_OnStreamUpdate;

			_twitchMonitor.Start();

		}

		private void TwitchMonitor_OnStreamUpdate(object sender, OnStreamUpdateArgs e)
		{
			//throw new NotImplementedException();
			Console.WriteLine($"User Id: {e.Stream.UserId}");


		}

		private void TwitchMonitor_OnStreamOnline(object sender, OnStreamOnlineArgs e)
		{
			_IsOnline = true;
		}

		private void TwitchMonitor_OnStreamOffline(object sender, OnStreamOfflineArgs e)
		{
			_IsOnline = false;
		}

		#endregion


		#region Connect

		/// <summary>
		/// Connects to the specified Twitch channel.
		/// </summary>
		/// <param name="channelName">Name of the Twitch channel to connect to.</param>
		/// <param name="logEvents">If set to <c>true</c> then events will be logged.</param>
		/// <returns></returns>
		private void ConnectTwitchClient(bool logEvents)
		{

			credentials = new ConnectionCredentials(_twitchSettings.ChannelName, _twitchSettings.AccessToken);

			_twitchClient.Initialize(credentials, _twitchSettings.ChannelName);

			if (logEvents)
				_twitchClient.OnLog += TwitchClient_OnLog;

			_twitchClient.OnConnected += TwitchClient_OnConnected;
			_twitchClient.OnDisconnected += TwitchClient_OnDisconnected;
			_twitchClient.OnChatCommandReceived += TwitchClient_OnChatCommandRecieved;
			_twitchClient.OnNewSubscriber += TwitchClient_OnNewSubscriber;
			_twitchClient.OnCommunitySubscription += TwitchClient_OnCommunitySubscription;
			_twitchClient.OnGiftedSubscription += TwitchClient_OnGiftedSubscription;
			_twitchClient.OnReSubscriber += TwitchClient_OnResubscriber;

			_twitchClient.Connect();

		}

		private void ConnectTwitchAPI()
		{
			_twitchAPI = new TwitchAPI();
			_twitchAPI.Settings.ClientId = _twitchSettings.ClientId;
			_twitchAPI.Settings.AccessToken = _twitchSettings.AccessToken;
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

			ChatCommand chatCommand = ChatCommand.RetrieveByCommand(_twitchSettings.ChannelName, e.Command.CommandText.ToLower(), _azureStorageSettings);
			if (chatCommand is null)
				chatCommand = ChatCommand.RetrieveByCommandAlias("BricksWithChad", e.Command.CommandText.ToLower(), _azureStorageSettings);

			if (chatCommand is not null)
			{
				if (!UserPermittedToExecuteCommand(chatCommand, e.Command.ChatMessage))
				{
					InvokeOnCommandNotPermitted(e, chatCommand);
				}
				else
				{
					// TODO: Handle no argument coming in					
					string responseMessage = string.Format(chatCommand.Response, e.Command.ArgumentsAsList.ToArray());

					switch (chatCommand.CommandResponseType)
					{
						case CommandResponseType.Say:
							_twitchClient.SendMessage(e.Command.ChatMessage.BotUsername, responseMessage);
							break;
						case CommandResponseType.Reply:
							_twitchClient.SendMessage(e.Command.ChatMessage.BotUsername, $"@{e.Command.ChatMessage.Username}: {responseMessage}");
							break;
						case CommandResponseType.Whisper:
							// TODO: Fix the whisper commands
							_twitchClient.SendWhisper(e.Command.ChatMessage.UserId, responseMessage);
							break;
					}

					InvokeOnCommandReceived(e, chatCommand, responseMessage);
				}

			}


		}

		private bool UserPermittedToExecuteCommand(ChatCommand chatCommand, ChatMessage chatMessage)
		{
			switch (chatCommand.UserPermission)
			{
				case UserPermission.Broadcaster:
					if (chatMessage.IsBroadcaster) return true;
					return false;
				case UserPermission.Moderator:
					if (chatMessage.IsBroadcaster || chatMessage.IsModerator) return true;
					return false;
				case UserPermission.VIP:
					if (chatMessage.IsBroadcaster || chatMessage.IsModerator || chatMessage.IsVip) return true;
					return false;
				case UserPermission.Subscriber:
					if (chatMessage.IsBroadcaster || chatMessage.IsModerator || chatMessage.IsSubscriber) return true;
					return false;
				case UserPermission.Everyone:
					return true;
				default:
					return false;
			}
		}

		/// <summary>
		/// Raised when the bot received a chat command.
		/// </summary>
		public EventHandler<OnCommandReceivedArgs> OnCommandReceived;

		/// <summary>
		/// Invokes the <see cref="OnCommandReceived"/> event.
		/// </summary>
		/// <param name="onChatCommandReceivedArgs">The on chat command received arguments.</param>
		/// <param name="chatCommand">The chat command.</param>
		/// <param name="returnedMessage">The returned message.</param>
		private void InvokeOnCommandReceived(OnChatCommandReceivedArgs onChatCommandReceivedArgs, ChatCommand chatCommand, string returnedMessage)
		{
			OnCommandReceived.Invoke(this, new OnCommandReceivedArgs(onChatCommandReceivedArgs, chatCommand, returnedMessage));
		}

		public EventHandler<OnCommandNotPermittedArgs> OnCommandNotPermitted;

		private void InvokeOnCommandNotPermitted(OnChatCommandReceivedArgs onChatCommandReceivedArgs, ChatCommand chatCommand)
		{
			OnCommandNotPermitted.Invoke(this, new OnCommandNotPermittedArgs(onChatCommandReceivedArgs, chatCommand));
		}


		#endregion

		#region Subscriptions

		private void TwitchClient_OnResubscriber(object sender, OnReSubscriberArgs e)
		{
			throw new NotImplementedException();
		}

		private void TwitchClient_OnGiftedSubscription(object sender, OnGiftedSubscriptionArgs e)
		{
			throw new NotImplementedException();
		}

		private void TwitchClient_OnCommunitySubscription(object sender, OnCommunitySubscriptionArgs e)
		{
			throw new NotImplementedException();
		}

		private void TwitchClient_OnNewSubscriber(object sender, OnNewSubscriberArgs e)
		{
			throw new NotImplementedException();
		}

		#endregion

	}

}