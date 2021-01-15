
namespace TaleLearnCode.TwitchCommander.UserControls
{
	partial class ProjectInfo
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.MetricsGroup = new Telerik.WinControls.UI.RadGroupBox();
			this.ProjectNameLabel = new Telerik.WinControls.UI.RadLabel();
			this.OofCount = new Telerik.WinControls.UI.RadLabel();
			this.SetProject = new Telerik.WinControls.UI.RadButton();
			this.ProjectName = new Telerik.WinControls.UI.RadTextBox();
			this.DroppedBrickCount = new Telerik.WinControls.UI.RadLabel();
			this.OofCountLabel = new Telerik.WinControls.UI.RadLabel();
			this.ProjectTimerLabel = new Telerik.WinControls.UI.RadLabel();
			this.ProjectTime = new Telerik.WinControls.UI.RadLabel();
			this.DroppedBrickCountLabel = new Telerik.WinControls.UI.RadLabel();
			((System.ComponentModel.ISupportInitialize)(this.MetricsGroup)).BeginInit();
			this.MetricsGroup.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.ProjectNameLabel)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.OofCount)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.SetProject)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ProjectName)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.DroppedBrickCount)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.OofCountLabel)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ProjectTimerLabel)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ProjectTime)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.DroppedBrickCountLabel)).BeginInit();
			this.SuspendLayout();
			// 
			// MetricsGroup
			// 
			this.MetricsGroup.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
			this.MetricsGroup.Controls.Add(this.ProjectNameLabel);
			this.MetricsGroup.Controls.Add(this.OofCount);
			this.MetricsGroup.Controls.Add(this.SetProject);
			this.MetricsGroup.Controls.Add(this.ProjectName);
			this.MetricsGroup.Controls.Add(this.DroppedBrickCount);
			this.MetricsGroup.Controls.Add(this.OofCountLabel);
			this.MetricsGroup.Controls.Add(this.ProjectTimerLabel);
			this.MetricsGroup.Controls.Add(this.ProjectTime);
			this.MetricsGroup.Controls.Add(this.DroppedBrickCountLabel);
			this.MetricsGroup.HeaderText = "Metrics";
			this.MetricsGroup.Location = new System.Drawing.Point(0, 0);
			this.MetricsGroup.Name = "MetricsGroup";
			this.MetricsGroup.Size = new System.Drawing.Size(207, 147);
			this.MetricsGroup.TabIndex = 29;
			this.MetricsGroup.Text = "Metrics";
			// 
			// ProjectNameLabel
			// 
			this.ProjectNameLabel.Location = new System.Drawing.Point(5, 22);
			this.ProjectNameLabel.Name = "ProjectNameLabel";
			this.ProjectNameLabel.Size = new System.Drawing.Size(76, 18);
			this.ProjectNameLabel.TabIndex = 10;
			this.ProjectNameLabel.Text = "Project Name:";
			// 
			// OofCount
			// 
			this.OofCount.Location = new System.Drawing.Point(105, 120);
			this.OofCount.Name = "OofCount";
			this.OofCount.Size = new System.Drawing.Size(29, 18);
			this.OofCount.TabIndex = 26;
			this.OofCount.Text = "0 / 0";
			// 
			// SetProject
			// 
			this.SetProject.Location = new System.Drawing.Point(112, 19);
			this.SetProject.Name = "SetProject";
			this.SetProject.Size = new System.Drawing.Size(84, 24);
			this.SetProject.TabIndex = 15;
			this.SetProject.Text = "Set Project";
			this.SetProject.Click += new System.EventHandler(this.SetProject_Click);
			// 
			// ProjectName
			// 
			this.ProjectName.Location = new System.Drawing.Point(5, 46);
			this.ProjectName.Name = "ProjectName";
			this.ProjectName.Size = new System.Drawing.Size(191, 20);
			this.ProjectName.TabIndex = 11;
			// 
			// DroppedBrickCount
			// 
			this.DroppedBrickCount.Location = new System.Drawing.Point(105, 96);
			this.DroppedBrickCount.Name = "DroppedBrickCount";
			this.DroppedBrickCount.Size = new System.Drawing.Size(29, 18);
			this.DroppedBrickCount.TabIndex = 24;
			this.DroppedBrickCount.Text = "0 / 0";
			// 
			// OofCountLabel
			// 
			this.OofCountLabel.Location = new System.Drawing.Point(6, 120);
			this.OofCountLabel.Name = "OofCountLabel";
			this.OofCountLabel.Size = new System.Drawing.Size(32, 18);
			this.OofCountLabel.TabIndex = 25;
			this.OofCountLabel.Text = "Oofs:";
			// 
			// ProjectTimerLabel
			// 
			this.ProjectTimerLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.ProjectTimerLabel.Location = new System.Drawing.Point(6, 72);
			this.ProjectTimerLabel.Name = "ProjectTimerLabel";
			this.ProjectTimerLabel.Size = new System.Drawing.Size(75, 18);
			this.ProjectTimerLabel.TabIndex = 12;
			this.ProjectTimerLabel.Text = "Project Timer:";
			// 
			// ProjectTime
			// 
			this.ProjectTime.Location = new System.Drawing.Point(105, 72);
			this.ProjectTime.Name = "ProjectTime";
			this.ProjectTime.Size = new System.Drawing.Size(62, 18);
			this.ProjectTime.TabIndex = 13;
			this.ProjectTime.Text = "00:00:00:00";
			// 
			// DroppedBrickCountLabel
			// 
			this.DroppedBrickCountLabel.Location = new System.Drawing.Point(6, 96);
			this.DroppedBrickCountLabel.Name = "DroppedBrickCountLabel";
			this.DroppedBrickCountLabel.Size = new System.Drawing.Size(85, 18);
			this.DroppedBrickCountLabel.TabIndex = 23;
			this.DroppedBrickCountLabel.Text = "Dropped Bricks:";
			// 
			// ProjectInfo
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.MetricsGroup);
			this.Name = "ProjectInfo";
			this.Size = new System.Drawing.Size(207, 148);
			((System.ComponentModel.ISupportInitialize)(this.MetricsGroup)).EndInit();
			this.MetricsGroup.ResumeLayout(false);
			this.MetricsGroup.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.ProjectNameLabel)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.OofCount)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.SetProject)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ProjectName)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.DroppedBrickCount)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.OofCountLabel)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ProjectTimerLabel)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ProjectTime)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.DroppedBrickCountLabel)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private Telerik.WinControls.UI.RadGroupBox MetricsGroup;
		private Telerik.WinControls.UI.RadLabel ProjectNameLabel;
		private Telerik.WinControls.UI.RadLabel OofCount;
		private Telerik.WinControls.UI.RadButton SetProject;
		private Telerik.WinControls.UI.RadTextBox ProjectName;
		private Telerik.WinControls.UI.RadLabel DroppedBrickCount;
		private Telerik.WinControls.UI.RadLabel OofCountLabel;
		private Telerik.WinControls.UI.RadLabel ProjectTimerLabel;
		private Telerik.WinControls.UI.RadLabel ProjectTime;
		private Telerik.WinControls.UI.RadLabel DroppedBrickCountLabel;
	}
}
