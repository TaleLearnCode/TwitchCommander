using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using TaleLearnCode.TwitchCommander.Events;
using TaleLearnCode.TwitchCommander.Extensions;
using Telerik.Charting;
using Telerik.WinControls.UI;
using TwitchLib.Api.Services.Events.LiveStreamMonitor;

namespace TaleLearnCode.TwitchCommander.UserControls
{

	public partial class StreamStats : UserControl
	{

		private WOPR _wopr = null;
		private OBSController _obsController = null;
		private string _gameId;

		public WOPR WOPR
		{
			get { return _wopr; }
			set
			{
				_wopr = value;
				if (_wopr != null)
				{
					_wopr.OnStreamUpdate += WOPR_OnStreamUpdate;
				}
			}
		}

		public OBSController OBSController
		{
			get { return _obsController; }
			set { _obsController = value; }
		}

		private void WOPR_OnStreamMetricsUpdated(object sender, OnStreamMetricsUpdatedArgs e)
		{
			DisplayMetrics(e.Followers, e.Subscribers);
		}

		private async void WOPR_OnStreamUpdate(object sender, OnStreamUpdateArgs e)
		{
			try
			{

				int subscribers = await _wopr.GetSubscriberCountAsync();
				int followers = await _wopr.GetFollowerCountAsync();

				DisplayStreamStats(e.Stream, followers, subscribers);
				radChartView2.Series[0].DataPoints.Add(new CategoricalDataPoint(e.Stream.ViewerCount));

				if (_obsController != null)
				{
					_obsController.SetText("Text: Viewers", e.Stream.ViewerCount.Display());
					_obsController.SetText("Text: Subscribers", subscribers.Display());
					_obsController.SetText("Text: Followers", followers.Display());
				}

			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				// TODO: Do something with this exception -- see if radChatView2 is crashing when displaying alerts
			}



		}

		delegate void DisplayStreamStatsCallback(TwitchLib.Api.Helix.Models.Streams.Stream stream, int follwerCount, int subscriberCount);

		private async void DisplayStreamStats(TwitchLib.Api.Helix.Models.Streams.Stream stream, int followerCount, int subscriberCount)
		{
			if (StreamId.InvokeRequired)
			{
				Invoke(new DisplayStreamStatsCallback(DisplayStreamStats), new object[] { stream, followerCount, subscriberCount });
			}
			else
			{
				StreamId.Text = stream.Id;
				StreamType.Text = stream.Type;
				if (stream.GameId != _gameId)
				{
					GameId.Text = await _wopr.GetGameNameAsync();
					_gameId = stream.GameId;
				}
				StartedAt.Text = stream.StartedAt.ToString("MM-dd-yyyy  HH:mm:ss");
				StreamTitle.Text = stream.Title;
				DisplayMetric(ViewersCount, stream.ViewerCount);
				DisplayMetrics(followerCount, subscriberCount);

				if (_obsController.IsConnected)
				{
					_obsController.SetText("Text: Subscribers", subscriberCount.Display());
					_obsController.SetText("Text: Follower", followerCount.Display());
					_obsController.SetText("Text: Views", followerCount.Display());
				}

			}
		}

		private void DisplayMetrics(int followerCount, int subscriberCount)
		{
			DisplayMetric(SubscribersCount, subscriberCount);
			DisplayMetric(FollowersCount, followerCount);
		}

		private void DisplayMetric(RadLabel radLabel, int newValue)
		{
			int currentValue = int.Parse(radLabel.Text, NumberStyles.AllowThousands);
			radLabel.ForeColor =
				(newValue > currentValue) ? Color.Green :
				(newValue < currentValue) ? Color.Red :
				Color.Black;
			radLabel.Text = newValue.Display();
		}

		public StreamStats()
		{
			InitializeComponent();
			FastLineSeries fastLineSeries = new FastLineSeries();
			radChartView2.Series.Add(fastLineSeries);
		}

	}

}