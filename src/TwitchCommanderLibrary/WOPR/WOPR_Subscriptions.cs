using System;
using TwitchLib.Client.Events;


namespace TaleLearnCode.TwitchCommander
{

	public partial class WOPR
	{

		/// <summary>
		/// Handles the <see cref="TwitchLib.Client.TwitchClient.OnReSubscriber"/> event.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="OnReSubscriberArgs"/> instance containing the event data.</param>
		private void TwitchClient_OnResubscriber(object sender, OnReSubscriberArgs e)
		{
			OnResubscription?.Invoke(this, e);
		}

		public EventHandler<OnReSubscriberArgs> OnResubscription;

		/// <summary>
		/// Handles the <see cref="TwitchLib.Client.TwitchClient.OnGiftedSubscription"/> event.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="OnGiftedSubscriptionArgs"/> instance containing the event data.</param>
		private void TwitchClient_OnGiftedSubscription(object sender, OnGiftedSubscriptionArgs e)
		{
			OnGiftedSubscription?.Invoke(this, e);
		}

		public EventHandler<OnGiftedSubscriptionArgs> OnGiftedSubscription;

		/// <summary>
		/// Handles the <see cref="TwitchLib.Client.TwitchClient.OnNewSubscriber"/> event.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="OnNewSubscriberArgs"/> instance containing the event data.</param>
		private void TwitchClient_OnNewSubscriber(object sender, OnNewSubscriberArgs e)
		{
			OnNewSubscriber?.Invoke(this, e);
		}

		public EventHandler<OnNewSubscriberArgs> OnNewSubscriber;

	}

}