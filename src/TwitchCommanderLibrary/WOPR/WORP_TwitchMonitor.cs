using System;
using TwitchLib.Api.Helix.Models.Streams;
using TwitchLib.Api.Services.Events.LiveStreamMonitor;

namespace TaleLearnCode.TwitchCommander
{

	public partial class WOPR
	{

		private Stream _stream;

		/// <summary>
		/// Handles the <see cref="TwitchLib.Api.Services.LiveStreamMonitorService.OnStreamUpdate"/> event.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="OnStreamUpdateArgs"/> instance containing the event data.</param>
		private void TwitchMonitor_OnStreamUpdate(object sender, OnStreamUpdateArgs e)
		{
			InvokeOnStreamUpdate(e);
		}

		private void InvokeOnStreamUpdate(OnStreamUpdateArgs onStreamUpdateArgs)
		{
			OnStreamUpdate?.Invoke(this, onStreamUpdateArgs);
		}

		public EventHandler<OnStreamUpdateArgs> OnStreamUpdate;

		/// <summary>
		/// Handles the <see cref="TwitchLib.Api.Services.LiveStreamMonitorService.OnStreamOnline"/> event.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="OnStreamOnlineArgs"/> instance containing the event data.</param>
		private void TwitchMonitor_OnStreamOnline(object sender, OnStreamOnlineArgs e)
		{
			_IsOnline = true;
			_stream = e.Stream;
		}

		/// <summary>
		/// Handles the <see cref="TwitchLib.Api.Services.LiveStreamMonitorService.OnStreamOffline"/> event.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="OnStreamOfflineArgs"/> instance containing the event data.</param>
		private void TwitchMonitor_OnStreamOffline(object sender, OnStreamOfflineArgs e)
		{
			_IsOnline = false;
			_stream = null;
		}

	}

}