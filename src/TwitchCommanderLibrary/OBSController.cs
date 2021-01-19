using OBSWebsocketDotNet;
using OBSWebsocketDotNet.Types;
using System.Threading;
using System.Threading.Tasks;

namespace TaleLearnCode.TwitchCommander
{

	/// <summary>
	/// Provides connection and controlling of an OBS instance via web sockets.
	/// </summary>
	public class OBSController
	{

		private OBSWebsocket _obs = new();

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

		/// <summary>
		/// Shows the specified scene item.
		/// </summary>
		/// <param name="sceneName">Name of the scene where the item to show is.</param>
		/// <param name="itemName">Name of the scene item to be displayed.</param>
		/// <param name="duration">If greater than 0, the number of seconds to display the scene item.</param>
		public void ShowSceneItem(string sceneName, string itemName, int duration = 0)
		{
			_obs.SetSceneItemProperties(GetSceneItemProperties(itemName, true), sceneName);

			if (duration > 0)
			{
				using CancellationTokenSource source = new();
				var t = Task.Run(async delegate
				{
					await Task.Delay(duration * 1000, source.Token);
					HideSceneItem(sceneName, itemName);
				});
				t.Wait();
			}
		}

		/// <summary>
		/// Hides the specified scene item.
		/// </summary>
		/// <param name="sceneName">Name of the scene where the item to hide is.</param>
		/// <param name="itemName">Name of the scene item to be hidden.</param>
		public void HideSceneItem(string sceneName, string itemName)
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

	}

}