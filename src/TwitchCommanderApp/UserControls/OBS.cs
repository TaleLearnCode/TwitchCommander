using OBSWebsocketDotNet.Types;
using System.Windows.Forms;
using TaleLearnCode.TwitchCommander.Extensions;

namespace TaleLearnCode.TwitchCommander.UserControls
{
	public partial class OBS : UserControl
	{
		private OBSController _obsController;

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
			DisplayTotalStreamTime(e.TotalStreamTime);
			DisplayOBSStatus(e);
		}

		delegate void DisplayTotalStreamTimeCallback(int totalStreamTime);

		private void DisplayTotalStreamTime(int totalStreamTime)
		{
			if (TotalStreamTime.InvokeRequired)
				Invoke(new DisplayTotalStreamTimeCallback(DisplayTotalStreamTime), new object[] { totalStreamTime });
			else
				TotalStreamTime.Text = totalStreamTime.Display();
		}

		delegate void DisplayOBSStatusCallback(StreamStatus streamStatus);

		private void DisplayOBSStatus(StreamStatus streamStatus)
		{
			if (KBits.InvokeRequired)
				Invoke(new DisplayOBSStatusCallback(DisplayOBSStatus), new object[] { streamStatus });
			else
			{
				KBits.Text = $"{streamStatus.KbitsPerSec.Display()} kbit/s";
				Bytes.Text = $"{streamStatus.BytesPerSec.Display()} bytes/s";
			}

		}

	}
}
