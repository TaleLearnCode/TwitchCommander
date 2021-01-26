using OBSWebsocketDotNet;
using OBSWebsocketDotNet.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TaleLearnCode.TwitchCommander
{

	/// <summary>
	/// Provides connection and controlling of an OBS instance via web sockets.
	/// </summary>
	public class OBSController
	{

		private OBSWebsocket _obs;
		private Dictionary<string, TextGDIPlusProperties> _textProperties = new();

		public OBSController()
		{
			_obs = new();
			_obs.StreamStatus += OBS_StreamStatus;
			_obs.Connected += OBS_Connected;
		}

		public EventHandler<EventArgs> OnConnected;

		private void OBS_Connected(object sender, EventArgs e)
		{
			List<SourceInfo> sources = _obs.GetSourcesList();
			foreach (SourceInfo source in sources)
				if (source.TypeID == "text_gdiplus_v2")
					_textProperties.Add(source.Name, _obs.GetTextGDIPlusProperties(source.Name));
			OnConnected?.Invoke(this, e);
		}

		/// <summary>
		/// Connects to the specifed OBS instance.
		/// </summary>
		/// <param name="url">The URL of the OBS web sockets.</param>
		/// <param name="password">The password for the OBS web sockets.</param>
		/// <exception cref="AuthFailureException">Thrown if authentication to the OBS web sockets fails.</exception>
		/// <exception cref="ErrorResponseException">Thrown when the the OBS web server responds with an error</exception>
		public void Connect(string url, string password)
		{
			if (!_obs.IsConnected)
				_obs.Connect(url, password);
		}

		/// <summary>
		/// Disconnects from the OBS web sockets.
		/// </summary>
		public void Disconnect()
		{
			_textProperties = new();
			if (_obs.IsConnected)
				_obs.Disconnect();
		}

		/// <summary>
		/// Gets a value indicating whether this instance is connected to OBS.
		/// </summary>
		/// <value>
		///   <c>true</c> if this instance is connected to OBS; otherwise, <c>false</c>.
		/// </value>
		public bool IsConnected => _obs.IsConnected;

		public bool DoesCurrentSceneContainSource(string sourceName)
		{
			OBSScene scene = _obs.GetCurrentScene();
			return scene.Items.Any(x => x.SourceName == sourceName);
		}

		/// <summary>
		/// Shows the specified scene item.
		/// </summary>
		/// <param name="sceneName">Name of the scene where the item to show is.</param>
		/// <param name="itemName">Name of the scene item to be displayed.</param>
		/// <param name="duration">If greater than 0, the number of seconds to display the scene item.</param>
		public void ShowSceneItem(string itemName, int duration = 0, string sceneName = null)
		{
			_obs.SetSceneItemProperties(GetSceneItemProperties(itemName, true), sceneName);
			if (duration > 0)
			{
				using CancellationTokenSource source = new();
				var t = Task.Run(async delegate
				{
					await Task.Delay(duration * 1000, source.Token);
					HideSceneItem(itemName, sceneName);
				});
				t.Wait();
			}
		}

		/// <summary>
		/// Hides the specified scene item.
		/// </summary>
		/// <param name="sceneName">Name of the scene where the item to hide is.</param>
		/// <param name="itemName">Name of the scene item to be hidden.</param>
		public void HideSceneItem(string itemName, string sceneName = null)
		{
			_obs.SetSceneItemProperties(GetSceneItemProperties(itemName, false), sceneName);
		}

		private SceneItemProperties GetSceneItemProperties(string itemName, bool visible)
		{
			return new SceneItemProperties()
			{
				ItemName = itemName,
				Item = itemName,
				Visible = visible
			};
		}

		public EventHandler<StreamStatus> OnOBSStatusChange;

		private void OBS_StreamStatus(OBSWebsocket sender, StreamStatus status)
		{
			OnOBSStatusChange?.Invoke(this, status);
		}

		public void SetText(string sourceName, string value)
		{
			if (_textProperties.ContainsKey(sourceName))
			{
				TextGDIPlusProperties textProperites = _textProperties[sourceName];
				textProperites.Text = value;
				_obs.SetTextGDIPlusProperties(textProperites);
			}
		}

	}

}