using System;
using TaleLearnCode.TwitchCommander.Events;
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
			OnCommandReceived.Invoke(this, new OnCommandReceivedArgs(onChatCommandReceivedArgs, chatCommand, returnedMessage));
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
			OnCommandNotPermitted.Invoke(this, new OnCommandNotPermittedArgs(onChatCommandReceivedArgs, chatCommand));
		}

	}

}
