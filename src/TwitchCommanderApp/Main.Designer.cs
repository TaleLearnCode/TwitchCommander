namespace TwitchCommanderApp
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
			this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
			((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
			this.SuspendLayout();
			// 
			// radLabel1
			// 
			this.radLabel1.Location = new System.Drawing.Point(13, 13);
			this.radLabel1.Name = "radLabel1";
			this.radLabel1.Size = new System.Drawing.Size(55, 18);
			this.radLabel1.TabIndex = 0;
			this.radLabel1.Text = "radLabel1";
			// 
			// Main
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(7, 15);
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(304, 274);
			this.Controls.Add(this.radLabel1);
			this.Name = "Main";
			// 
			// 
			// 
			this.RootElement.ApplyShapeToControl = true;
			this.Text = "Main";
			((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

		#endregion

		private Telerik.WinControls.UI.RadLabel radLabel1;
	}
}
