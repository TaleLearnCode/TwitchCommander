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
			this.TwitchClientLog = new Telerik.WinControls.UI.RadListControl();
			this.ConnectBricksWithChad = new Telerik.WinControls.UI.RadButton();
			this.ConnectTaleLearnCode = new Telerik.WinControls.UI.RadButton();
			this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
			this.radLabel2 = new Telerik.WinControls.UI.RadLabel();
			((System.ComponentModel.ISupportInitialize)(this.TwitchClientLog)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ConnectBricksWithChad)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ConnectTaleLearnCode)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.radLabel2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
			this.SuspendLayout();
			// 
			// TwitchClientLog
			// 
			this.TwitchClientLog.AutoScroll = true;
			this.TwitchClientLog.EnableAlternatingItemColor = true;
			this.TwitchClientLog.Location = new System.Drawing.Point(12, 12);
			this.TwitchClientLog.Name = "TwitchClientLog";
			this.TwitchClientLog.SelectionMode = System.Windows.Forms.SelectionMode.None;
			this.TwitchClientLog.Size = new System.Drawing.Size(297, 553);
			this.TwitchClientLog.TabIndex = 0;
			// 
			// ConnectBricksWithChad
			// 
			this.ConnectBricksWithChad.Location = new System.Drawing.Point(406, 43);
			this.ConnectBricksWithChad.Name = "ConnectBricksWithChad";
			this.ConnectBricksWithChad.Size = new System.Drawing.Size(110, 24);
			this.ConnectBricksWithChad.TabIndex = 5;
			this.ConnectBricksWithChad.Text = "Connect";
			this.ConnectBricksWithChad.Click += new System.EventHandler(this.ConnectBricksWithChad_Click);
			// 
			// ConnectTaleLearnCode
			// 
			this.ConnectTaleLearnCode.Location = new System.Drawing.Point(406, 13);
			this.ConnectTaleLearnCode.Name = "ConnectTaleLearnCode";
			this.ConnectTaleLearnCode.Size = new System.Drawing.Size(110, 24);
			this.ConnectTaleLearnCode.TabIndex = 6;
			this.ConnectTaleLearnCode.Text = "Connect";
			this.ConnectTaleLearnCode.Click += new System.EventHandler(this.ConnectTaleLearnCode_Click);
			// 
			// radLabel1
			// 
			this.radLabel1.Location = new System.Drawing.Point(316, 46);
			this.radLabel1.Name = "radLabel1";
			this.radLabel1.Size = new System.Drawing.Size(84, 18);
			this.radLabel1.TabIndex = 7;
			this.radLabel1.Text = "BricksWithChad";
			// 
			// radLabel2
			// 
			this.radLabel2.Location = new System.Drawing.Point(316, 16);
			this.radLabel2.Name = "radLabel2";
			this.radLabel2.Size = new System.Drawing.Size(80, 18);
			this.radLabel2.TabIndex = 8;
			this.radLabel2.Text = "TaleLearnCode";
			// 
			// Main
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(7, 15);
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(950, 577);
			this.Controls.Add(this.radLabel2);
			this.Controls.Add(this.radLabel1);
			this.Controls.Add(this.ConnectTaleLearnCode);
			this.Controls.Add(this.ConnectBricksWithChad);
			this.Controls.Add(this.TwitchClientLog);
			this.Name = "Main";
			// 
			// 
			// 
			this.RootElement.ApplyShapeToControl = true;
			this.Text = "Main";
			((System.ComponentModel.ISupportInitialize)(this.TwitchClientLog)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ConnectBricksWithChad)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ConnectTaleLearnCode)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.radLabel2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

		#endregion

		private Telerik.WinControls.UI.RadListControl TwitchClientLog;
		private Telerik.WinControls.UI.RadButton ConnectBricksWithChad;
		private Telerik.WinControls.UI.RadButton ConnectTaleLearnCode;
		private Telerik.WinControls.UI.RadLabel radLabel1;
		private Telerik.WinControls.UI.RadLabel radLabel2;
	}
}
