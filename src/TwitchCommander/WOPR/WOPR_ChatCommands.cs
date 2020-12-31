using System;
using System.Linq;
using TaleLearnCode.TwitchCommander.AzureStorage;
using TaleLearnCode.TwitchCommander.Events;
using TaleLearnCode.TwitchCommander.Extensions;
using TaleLearnCode.TwitchCommander.Models;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;

namespace TaleLearnCode.TwitchCommander
{
	public partial class WOPR
	{

		/// <summary>
		/// Handles the <see cref="TwitchLib.Client.TwitchClient.OnChatCommandReceived"/> event.
		/// </summary>
		/// <param name="sender">Reference to the object sending the event.</param>
		/// <param name="e">A <see cref="OnChatCommandReceivedArgs"/> containing details about the received chat command.</param>
		private void TwitchClient_OnChatCommandReceived(object sender, OnChatCommandReceivedArgs e)
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
				else if (chatCommand.GlobalCooldown > 0 && IsCommandTimedOut(chatCommand, ChatCommandTimeoutType.Global, e.Command.ChatMessage.Username, e.Command.ChatMessage.Message))
				{
					// Globally timed out - ignore the chat command (event raised in IsCommandTimedOut)
				}
				else if (chatCommand.UserCooldown > 0 && IsCommandTimedOut(chatCommand, ChatCommandTimeoutType.Chatter, e.Command.ChatMessage.Username, e.Command.ChatMessage.Message))
				{
					// Timed out for the user - ignore the chat command (event raised in IsCommandTimedOut)
				}
				else
				{

					if (_IsOnline && !chatCommand.IsEnabledWhenStreaming)
						_twitchClient.SendMessage(e.Command.ChatMessage.BotUsername, $"The {chatCommand.CommandName} is not available while {e.Command.ChatMessage.Channel} is broadcasting.");
					if (!_IsOnline && !chatCommand.IsEnabledWhenNotStreaming)
						_twitchClient.SendMessage(e.Command.ChatMessage.BotUsername, $"The {chatCommand.CommandName} is only available when {e.Command.ChatMessage.Channel} is broadcasting.");

					string responseMessage;
					if (e.Command.ArgumentsAsList.Any())
					{
						responseMessage = string.Format(chatCommand.Response, e.Command.ArgumentsAsList.ToArray());
					}
					else if (chatCommand.Response.Contains('{'))
					{
						responseMessage = $"The '{e.Command.CommandText}' requires {chatCommand.Response.Count(x => x == '{')} argument(s).";
						chatCommand.CommandResponseType = CommandResponseType.Reply;
					}
					else
					{
						responseMessage = chatCommand.Response;
					}



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

					ChatCommandActivityEntity.Save(_azureStorageSettings, _twitchSettings.ChannelName, e.Command.ChatMessage.Username, chatCommand.CommandName, e.Command.ChatMessage.Message, responseMessage);
					InvokeOnCommandReceived(e, chatCommand, responseMessage);
				}

			}


		}

		/// <summary>
		/// Determines whether a chat command user is authorized to perform the command.
		/// </summary>
		/// <param name="chatCommand">A <see cref="ChatCommand"/> representing the received chat command.</param>
		/// <param name="chatMessage">A <see cref="ChatMessage"/> representing the requested chat command.</param>
		/// <returns><c>true</c> if the user is authorized to perform the chat command; otherwise, <c>false</c>.</returns>
		private static bool UserPermittedToExecuteCommand(ChatCommand chatCommand, ChatMessage chatMessage)
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

		private bool IsCommandTimedOut(ChatCommand chatCommand, ChatCommandTimeoutType chatCommandTimeoutType, string chatter, string request)
		{
			ChatCommandActivity chatCommandActivity = ChatCommandActivityEntity.GetLastCommandRequest(_azureStorageSettings, _twitchSettings.ChannelName, chatCommand.CommandName, chatter);
			bool timedOut = false;
			if (chatCommandActivity != null)
				timedOut = chatCommandActivity.RequestTime + ((chatCommandTimeoutType == ChatCommandTimeoutType.Global) ? chatCommand.GlobalCooldown : chatCommand.UserCooldown) >= DateTime.UtcNow.ToUnixTimeSeconds();
			if (timedOut)
			{
				ChatCommandActivityEntity.Save(_azureStorageSettings, _twitchSettings.ChannelName, chatter, chatCommand.CommandName, request, string.Empty, chatCommandActivity.RequestTime, ChatCommandResult.GlobalCooldown);
				InvokeOnChatCommandTimedOut(chatCommand, chatCommandActivity, chatCommandTimeoutType);
			}
			return timedOut;
		}

		/// <summary>
		/// Raised when the bot receives a chat command.
		/// </summary>
		public EventHandler<OnCommandReceivedArgs> OnCommandReceived;

		/// <summary>
		/// Invokes the <see cref="OnCommandReceived"/> event.
		/// </summary>
		/// <param name="chatCommand">A <see cref="ChatCommand"/> representing the received chat command.</param>
		/// <param name="chatMessage">A <see cref="ChatMessage"/> representing the requested chat command.</param>
		/// <param name="returnedMessage">A <c>string</c> representing the message sent back to chat.</param>
		private void InvokeOnCommandReceived(OnChatCommandReceivedArgs onChatCommandReceivedArgs, ChatCommand chatCommand, string returnedMessage)
		{
			OnCommandReceived?.Invoke(this, new OnCommandReceivedArgs(onChatCommandReceivedArgs, chatCommand, returnedMessage));
		}

		/// <summary>
		/// Raised when the chat user is not permitted to execute the command.
		/// </summary>
		public EventHandler<OnCommandNotPermittedArgs> OnCommandNotPermitted;

		/// <summary>
		/// Invokes the on command not permitted event.
		/// </summary>
		/// <param name="onChatCommandReceivedArgs">The <see cref="OnChatCommandReceivedArgs"/> from the OnChatCommandReceived event.</param>
		/// <param name="chatCommand">The <see cref="ChatCommand"/> the user attempted to execute.</param>
		/// <returns></returns>
		private void InvokeOnCommandNotPermitted(OnChatCommandReceivedArgs onChatCommandReceivedArgs, ChatCommand chatCommand)
		{
			OnCommandNotPermitted?.Invoke(this, new OnCommandNotPermittedArgs(onChatCommandReceivedArgs, chatCommand));
		}

		public EventHandler<OnChatCommandTimedOutArgs> OnChatCommandTimedOut;

		private void InvokeOnChatCommandTimedOut(ChatCommand chatCommand, ChatCommandActivity chatCommandActivity, ChatCommandTimeoutType chatCommandTimeoutType)
		{
			var x = new OnChatCommandTimedOutArgs(chatCommand, chatCommandActivity, chatCommandTimeoutType);
			OnChatCommandTimedOut?.Invoke(this, x);
		}

	}

}
