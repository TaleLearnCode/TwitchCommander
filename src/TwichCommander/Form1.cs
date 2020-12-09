using System;
using System.Windows.Forms;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Events;

namespace TwichCommander
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		//private void button1_Click(object sender, System.EventArgs e)
		//{
		//	ConnectToTwitchAPI();
		//}



		ConnectionCredentials credentials = new ConnectionCredentials(Settings.ChannelName, Settings.AccessToken);
		TwitchClient twitchClient = new();

		private void ConnectToTwitchAPI()
		{

			twitchClient.Initialize(credentials, Settings.ChannelName);

			twitchClient.OnConnected += TwitchClient_OnConnected;
			twitchClient.OnMessageReceived += TwitchClient_OnMessageReceived;
			twitchClient.OnDisconnected += TwitchClient_OnDisconnected;

			twitchClient.Connect();

		}

		private void TwitchClient_OnDisconnected(object sender, OnDisconnectedEventArgs e)
		{
			MessageBox.Show("Disconnected from the Twich API");
		}

		private void TwitchClient_OnMessageReceived(object sender, OnMessageReceivedArgs e)
		{
			ChatBox.BeginUpdate();

			ChatBox.Invoke();


			ChatBox.Items.Add($"{e.ChatMessage.Username}: {e.ChatMessage.Message}");
			ChatBox.EndUpdate();
		}

		private void TwitchClient_OnConnected(object sender, OnConnectedArgs e)
		{
			MessageBox.Show("Connected to the Twitch API");
		}

		private void ConnectToTwitch_Click(object sender, EventArgs e)
		{
			if (ConnectToTwitch.Text == "Connect to Twitch")
			{
				ConnectToTwitchAPI();
				ConnectToTwitch.Text = "Disconect from Twitch";
			}
			else
			{
				twitchClient.Disconnect();
				ConnectToTwitch.Text = "Connect to Twitch";
			}
		}
	}
}
