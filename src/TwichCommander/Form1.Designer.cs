
namespace TwichCommander
{
	partial class Form1
	{
		/// <summary>
		///  Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		///  Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.ChatBox = new System.Windows.Forms.ListBox();
			this.ConnectToTwitch = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// ChatBox
			// 
			this.ChatBox.FormattingEnabled = true;
			this.ChatBox.ItemHeight = 15;
			this.ChatBox.Location = new System.Drawing.Point(12, 12);
			this.ChatBox.Name = "ChatBox";
			this.ChatBox.Size = new System.Drawing.Size(241, 424);
			this.ChatBox.TabIndex = 0;
			// 
			// ConnectToTwitch
			// 
			this.ConnectToTwitch.Location = new System.Drawing.Point(260, 13);
			this.ConnectToTwitch.Name = "ConnectToTwitch";
			this.ConnectToTwitch.Size = new System.Drawing.Size(146, 23);
			this.ConnectToTwitch.TabIndex = 1;
			this.ConnectToTwitch.Text = "Connect to Twitch";
			this.ConnectToTwitch.UseVisualStyleBackColor = true;
			this.ConnectToTwitch.Click += new System.EventHandler(this.ConnectToTwitch_Click);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.ConnectToTwitch);
			this.Controls.Add(this.ChatBox);
			this.Name = "Form1";
			this.Text = "Form1";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListBox ChatBox;
		private System.Windows.Forms.Button ConnectToTwitch;
	}
}

