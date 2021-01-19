using OBSWebsocketDotNet.Types;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using TaleLearnCode.TwitchCommander.Extensions;
using Telerik.WinControls.UI;

namespace TaleLearnCode.TwitchCommander.UserControls
{
	public partial class OBS : UserControl
	{
		private OBSController _obsController;
		private int _skippedFrames = 0;
		private int _renderMissedFrames = 0;
		private int _droppedFrames = 0;

		public OBS()
		{
			InitializeComponent();
		}

		public OBSController OBSControler
		{
			get { return _obsController; }
			set
			{
				_obsController = value;
				if (_obsController != null)
					_obsController.OnOBSStatusChange += OnOBSStatusChange;
			}
		}

		private void OnOBSStatusChange(object sender, StreamStatus e)
		{
			DisplayOBSStatus(e);
		}

		delegate void DisplayOBSStatusCallback(StreamStatus streamStatus);

		private void DisplayOBSStatus(StreamStatus streamStatus)
		{
			if (Streaming.InvokeRequired)
				Invoke(new DisplayOBSStatusCallback(DisplayOBSStatus), new object[] { streamStatus });
			else
			{
				Streaming.Text = (streamStatus.Streaming) ? "Active" : "Inactive";
				Recording.Text = (streamStatus.Recording) ? "Active" : "Inactive";
				ReplayBuffer.Text = (streamStatus.ReplayBufferActive) ? "Active" : "Inactive";

				Framerate.Text = $"{streamStatus.FPS} fps";
				KBits.Text = $"{streamStatus.KbitsPerSec.Display()} kbit/s";
				StreamTime.Text = streamStatus.StreamTime;

				DisplayFramesStat(DroppedFrames, _droppedFrames, streamStatus.DroppedFrames, streamStatus.TotalFrames);
				_droppedFrames = streamStatus.DroppedFrames;
				DisplayFramesStat(MissedFrames, _renderMissedFrames, streamStatus.RenderMissedFrames, streamStatus.TotalFrames);
				_renderMissedFrames = streamStatus.RenderMissedFrames;
				DisplayFramesStat(SkippedFrames, _skippedFrames, streamStatus.SkippedFrames, streamStatus.TotalFrames);
				_skippedFrames = streamStatus.SkippedFrames;

				CPU.Text = $"{streamStatus.CPU.ToString("N1", CultureInfo.InvariantCulture)}%";
				Strain.Text = $"{streamStatus.Strain.ToString("N1", CultureInfo.InvariantCulture)}%";

			}

		}

		private void DisplayFramesStat(RadLabel radLabel, int currentValue, int newValue, int totalValue)
		{
			radLabel.ForeColor = (newValue > currentValue) ? Color.Red : Color.Black;
			radLabel.Text = $"{newValue} / {totalValue} ({(newValue / totalValue).ToString("N:1", CultureInfo.InvariantCulture)}%)";
		}



	}
}
