using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Threading.Tasks;
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

		//BindingList<MetricCounts> myList;

		LineSeries viewerLineSeries = new();
		LineSeries subscriberLineSeries = new();
		LineSeries followerLineSeries = new();

		private WOPR _wopr = null;
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
					//_wopr.OnStreamMetricsUpdated += WOPR_OnStreamMetricsUpdated;
				}
			}
		}

		public async Task RefreshStats()
		{
			if (_wopr != null)
			{
				//DisplaySubscribersCount(await _wopr.GetSubscriberCountAsync());
				//DisplayFollowersCount(await _wopr.GetFollowerCountAsync());
			}
		}

		private void WOPR_OnStreamMetricsUpdated(object sender, OnStreamMetricsUpdatedArgs e)
		{
			DisplaySubscribersCount(e.Subscribers);
			DisplayFollowersCount(e.Followers);
		}

		private async void WOPR_OnStreamUpdate(object sender, OnStreamUpdateArgs e)
		{

			int subscribers = await _wopr.GetSubscriberCountAsync();
			int followers = await _wopr.GetFollowerCountAsync();
		
			DisplayStreamId(e.Stream.Id);
			DisplayGameId(e.Stream.GameId);
			DisplayStreamType(e.Stream.Type);
			DisplayStreamTitle(e.Stream.Title);
			DisplayViewerCount(e.Stream.ViewerCount);
			DisplayStartedAt(e.Stream.StartedAt);
			DisplaySubscribersCount(subscribers);
			DisplayFollowersCount(followers);

			////myList = new BindingList<MetricCounts>();
			////myList.Add(new MetricCounts(e.Stream.ViewerCount, "Viewers"));
			////myList.Add(new MetricCounts(subscribers, "Subscribers"));
			////myList.Add(new MetricCounts(followers, "Followers"));
			////BarSeries barSeria = new BarSeries();
			////radChartView1.Series.Add(barSeria);
			////barSeria.DataSource = myList;
			////barSeria.ValueMember = "MyInt";
			////barSeria.CategoryMember = "MyString";

			////LineSeries lineSeries = new LineSeries();
			////lineSeries.DataPoints.Add(new CategoricalDataPoint(20, "Jan"));
			////lineSeries.DataPoints.Add(new CategoricalDataPoint(22, "Apr"));
			////lineSeries.DataPoints.Add(new CategoricalDataPoint(12, "Jul"));
			////lineSeries.DataPoints.Add(new CategoricalDataPoint(19, "Oct"));
			////this.radChartView1.Series.Add(lineSeries);
			////LineSeries lineSeries2 = new LineSeries();
			////lineSeries2.DataPoints.Add(new CategoricalDataPoint(18, "Jan"));
			////lineSeries2.DataPoints.Add(new CategoricalDataPoint(15, "Apr"));
			////lineSeries2.DataPoints.Add(new CategoricalDataPoint(17, "Jul"));
			////lineSeries2.DataPoints.Add(new CategoricalDataPoint(22, "Oct"));
			////this.radChartView1.Series.Add(lineSeries2);


			//followerLineSeries.DataPoints.Add(new CategoricalDataPoint(followers, DateTime.UtcNow));
			//subscriberLineSeries.DataPoints.Add(new CategoricalDataPoint(subscribers, DateTime.UtcNow));
			//viewerLineSeries.DataPoints.Add(new CategoricalDataPoint(e.Stream.ViewerCount, DateTime.UtcNow));

			//radChartView1.Series.Clear();
			//radChartView1.Series.Add(followerLineSeries);
			//radChartView1.Series.Add(subscriberLineSeries);
			//radChartView1.Series.Add(subscriberLineSeries);

		}

		public void Disable()
		{

		}

		delegate void DisplayStreamIdCallback(string streamId);

		private void DisplayStreamId(string streamId)
		{
			if (StreamId.InvokeRequired)
				Invoke(new DisplayStreamIdCallback(DisplayStreamId), new object[] { streamId });
			else
				StreamId.Text = streamId;
		}

		delegate void DisplayStreamTypeCallback(string streamType);

		private void DisplayStreamType(string streamType)
		{
			if (StreamType.InvokeRequired)
				Invoke(new DisplayStreamTypeCallback(DisplayStreamType), new object[] { streamType });
			else
				StreamType.Text = streamType;
		}

		delegate void DisplayGameIdCallback(string gameId);

		private async void DisplayGameId(string gameId)
		{
			if (GameId.InvokeRequired)
			{
				Invoke(new DisplayGameIdCallback(DisplayGameId), new object[] { gameId });
			}
			else
			{
				if (gameId != _gameId)
				{
					GameId.Text = await _wopr.GetGameNameAsync();
					_gameId = gameId;
				}
			}
		}

		delegate void DisplayStartedAtCallback(DateTime startedAt);

		private void DisplayStartedAt(DateTime startedAt)
		{
			if (StartedAt.InvokeRequired)
				Invoke(new DisplayStartedAtCallback(DisplayStartedAt), new object[] { startedAt });
			else
				StartedAt.Text = startedAt.ToString("MM-dd-yyyy  HH:mm:ss");
		}

		delegate void DisplayStreamTitleCallback(string streamTitle);

		private void DisplayStreamTitle(string streamTitle)
		{
			if (StreamTitle.InvokeRequired)
				Invoke(new DisplayStreamTitleCallback(DisplayStreamTitle), new object[] { streamTitle });
			else
				StreamTitle.Text = streamTitle;
		}

		delegate void DisplayViewerCountCallback(int viewers);

		private void DisplayViewerCount(int viewers)
		{
			if (ViewersCount.InvokeRequired)
				Invoke(new DisplayViewerCountCallback(DisplayViewerCount), new object[] { viewers });
			else
				DisplayMetric(ViewersCount, viewers);
		}

		delegate void DisplaySubscribersCountCallback(int subscribers);

		private void DisplaySubscribersCount(int subscribers)
		{
			if (SubscribersCount.InvokeRequired)
				Invoke(new DisplaySubscribersCountCallback(DisplaySubscribersCount), new object[] { subscribers });
			else
				DisplayMetric(SubscribersCount, subscribers);
		}

		delegate void DisplayFollowersCountCallback(int followers);

		private void DisplayFollowersCount(int followers)
		{
			if (FollowersCount.InvokeRequired)
				Invoke(new DisplayFollowersCountCallback(DisplayFollowersCount), new object[] { followers });
			else
				DisplayMetric(FollowersCount, followers);
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
		}
	}
}
