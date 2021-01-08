namespace TaleLearnCode.TwitchCommander
{
	partial class Main
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
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
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.ViewSelector = new Telerik.WinControls.UI.RadDropDownList();
			this.TwitchClientLog = new Telerik.WinControls.UI.RadListControl();
			this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
			this.ConnectedToTaleLearnCode = new Telerik.WinControls.UI.RadToggleSwitch();
			this.radLabel2 = new Telerik.WinControls.UI.RadLabel();
			this.ConnectedToBricksWithChad = new Telerik.WinControls.UI.RadToggleSwitch();
			this.radLabel3 = new Telerik.WinControls.UI.RadLabel();
			((System.ComponentModel.ISupportInitialize)(this.ViewSelector)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.TwitchClientLog)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ConnectedToTaleLearnCode)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.radLabel2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ConnectedToBricksWithChad)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.radLabel3)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
			this.SuspendLayout();
			// 
			// ViewSelector
			// 
			this.ViewSelector.Location = new System.Drawing.Point(389, 12);
			this.ViewSelector.Name = "ViewSelector";
			this.ViewSelector.Size = new System.Drawing.Size(125, 20);
			this.ViewSelector.TabIndex = 0;
			this.ViewSelector.SelectedIndexChanged += new Telerik.WinControls.UI.Data.PositionChangedEventHandler(this.ViewSelector_SelectedIndexChanged);
			// 
			// TwitchClientLog
			// 
			this.TwitchClientLog.Location = new System.Drawing.Point(12, 12);
			this.TwitchClientLog.Name = "TwitchClientLog";
			this.TwitchClientLog.Size = new System.Drawing.Size(264, 498);
			this.TwitchClientLog.TabIndex = 2;
			// 
			// radLabel1
			// 
			this.radLabel1.Location = new System.Drawing.Point(282, 12);
			this.radLabel1.Name = "radLabel1";
			this.radLabel1.Size = new System.Drawing.Size(101, 18);
			this.radLabel1.TabIndex = 3;
			this.radLabel1.Text = "Displayed Channel:";
			// 
			// ConnectedToTaleLearnCode
			// 
			this.ConnectedToTaleLearnCode.Location = new System.Drawing.Point(854, 12);
			this.ConnectedToTaleLearnCode.Name = "ConnectedToTaleLearnCode";
			this.ConnectedToTaleLearnCode.OffText = "Disconnected";
			this.ConnectedToTaleLearnCode.OnText = "Connected";
			this.ConnectedToTaleLearnCode.Size = new System.Drawing.Size(125, 20);
			this.ConnectedToTaleLearnCode.TabIndex = 4;
			this.ConnectedToTaleLearnCode.Value = false;
			this.ConnectedToTaleLearnCode.ValueChanged += new System.EventHandler(this.ConnectedToTaleLearnCode_ValueChanged);
			// 
			// radLabel2
			// 
			this.radLabel2.Location = new System.Drawing.Point(532, 13);
			this.radLabel2.Name = "radLabel2";
			this.radLabel2.Size = new System.Drawing.Size(84, 18);
			this.radLabel2.TabIndex = 5;
			this.radLabel2.Text = "BricksWithChad";
			// 
			// ConnectedToBricksWithChad
			// 
			this.ConnectedToBricksWithChad.Location = new System.Drawing.Point(622, 12);
			this.ConnectedToBricksWithChad.Name = "ConnectedToBricksWithChad";
			this.ConnectedToBricksWithChad.OffText = "Disconnected";
			this.ConnectedToBricksWithChad.OnText = "Connected";
			this.ConnectedToBricksWithChad.Size = new System.Drawing.Size(125, 20);
			this.ConnectedToBricksWithChad.TabIndex = 6;
			this.ConnectedToBricksWithChad.Value = false;
			this.ConnectedToBricksWithChad.ValueChanged += new System.EventHandler(this.ConnectedToBricksWithChad_ValueChanged);
			// 
			// radLabel3
			// 
			this.radLabel3.Location = new System.Drawing.Point(768, 12);
			this.radLabel3.Name = "radLabel3";
			this.radLabel3.Size = new System.Drawing.Size(80, 18);
			this.radLabel3.TabIndex = 7;
			this.radLabel3.Text = "TaleLearnCode";
			// 
			// Main
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(7, 15);
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1220, 486);
			this.Controls.Add(this.radLabel3);
			this.Controls.Add(this.ConnectedToBricksWithChad);
			this.Controls.Add(this.radLabel2);
			this.Controls.Add(this.ConnectedToTaleLearnCode);
			this.Controls.Add(this.radLabel1);
			this.Controls.Add(this.TwitchClientLog);
			this.Controls.Add(this.ViewSelector);
			this.Name = "Main";
			// 
			// 
			// 
			this.RootElement.ApplyShapeToControl = true;
			this.Text = "Twitch Commander";
			((System.ComponentModel.ISupportInitialize)(this.ViewSelector)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.TwitchClientLog)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ConnectedToTaleLearnCode)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.radLabel2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ConnectedToBricksWithChad)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.radLabel3)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private Telerik.WinControls.UI.RadDropDownList ViewSelector;
		private Telerik.WinControls.UI.RadListControl TwitchClientLog;
		private Telerik.WinControls.UI.RadLabel radLabel1;
		private Telerik.WinControls.UI.RadToggleSwitch ConnectedToTaleLearnCode;
		private Telerik.WinControls.UI.RadLabel radLabel2;
		private Telerik.WinControls.UI.RadToggleSwitch ConnectedToBricksWithChad;
		private Telerik.WinControls.UI.RadLabel radLabel3;
	}
}
