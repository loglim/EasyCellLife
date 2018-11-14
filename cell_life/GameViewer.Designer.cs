namespace LogLim.EasyCellLife
{
    partial class GameViewer
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GameViewer));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.GamePictureBox = new System.Windows.Forms.PictureBox();
            this.StartButton = new System.Windows.Forms.Button();
            this.DeleteButton = new System.Windows.Forms.Button();
            this.ResetButton = new System.Windows.Forms.Button();
            this.PauseButton = new System.Windows.Forms.Button();
            this.lblGeneration = new System.Windows.Forms.Label();
            this.SpeedSlider = new System.Windows.Forms.TrackBar();
            this.label2 = new System.Windows.Forms.Label();
            this.SpeedLabel = new System.Windows.Forms.Label();
            this.pbHistoryGraph = new System.Windows.Forms.PictureBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.FileButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.OpenButton = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveAsButton = new System.Windows.Forms.ToolStripMenuItem();
            this.ExportButton = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ExitButton = new System.Windows.Forms.ToolStripMenuItem();
            this.AboutButton = new System.Windows.Forms.ToolStripButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.StatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.VersionLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.ThemeComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.GamePictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpeedSlider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbHistoryGraph)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // GamePictureBox
            // 
            this.GamePictureBox.Location = new System.Drawing.Point(12, 35);
            this.GamePictureBox.Name = "GamePictureBox";
            this.GamePictureBox.Size = new System.Drawing.Size(424, 389);
            this.GamePictureBox.TabIndex = 0;
            this.GamePictureBox.TabStop = false;
            this.GamePictureBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.PictureBox1_MouseClick);
            // 
            // StartButton
            // 
            this.StartButton.Location = new System.Drawing.Point(12, 430);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(75, 23);
            this.StartButton.TabIndex = 2;
            this.StartButton.Text = "Start";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.BtnStart_Click);
            // 
            // DeleteButton
            // 
            this.DeleteButton.Enabled = false;
            this.DeleteButton.Location = new System.Drawing.Point(174, 430);
            this.DeleteButton.Name = "DeleteButton";
            this.DeleteButton.Size = new System.Drawing.Size(75, 23);
            this.DeleteButton.TabIndex = 3;
            this.DeleteButton.Text = "Delete";
            this.DeleteButton.UseVisualStyleBackColor = true;
            this.DeleteButton.Click += new System.EventHandler(this.BtnDelete_Click);
            // 
            // ResetButton
            // 
            this.ResetButton.Enabled = false;
            this.ResetButton.Location = new System.Drawing.Point(93, 430);
            this.ResetButton.Name = "ResetButton";
            this.ResetButton.Size = new System.Drawing.Size(75, 23);
            this.ResetButton.TabIndex = 4;
            this.ResetButton.Text = "Reset";
            this.ResetButton.UseVisualStyleBackColor = true;
            this.ResetButton.Click += new System.EventHandler(this.BtnReset_Click);
            // 
            // PauseButton
            // 
            this.PauseButton.Enabled = false;
            this.PauseButton.Location = new System.Drawing.Point(255, 430);
            this.PauseButton.Name = "PauseButton";
            this.PauseButton.Size = new System.Drawing.Size(75, 23);
            this.PauseButton.TabIndex = 8;
            this.PauseButton.Text = "Pause";
            this.PauseButton.UseVisualStyleBackColor = true;
            this.PauseButton.Click += new System.EventHandler(this.BtnPause_Click);
            // 
            // lblGeneration
            // 
            this.lblGeneration.AutoSize = true;
            this.lblGeneration.Location = new System.Drawing.Point(442, 216);
            this.lblGeneration.Name = "lblGeneration";
            this.lblGeneration.Size = new System.Drawing.Size(71, 13);
            this.lblGeneration.TabIndex = 9;
            this.lblGeneration.Text = "Generation: 0";
            // 
            // SpeedSlider
            // 
            this.SpeedSlider.Location = new System.Drawing.Point(442, 379);
            this.SpeedSlider.Maximum = 100;
            this.SpeedSlider.Name = "SpeedSlider";
            this.SpeedSlider.Size = new System.Drawing.Size(170, 45);
            this.SpeedSlider.TabIndex = 10;
            this.SpeedSlider.Scroll += new System.EventHandler(this.SpeedSlider_Scroll);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(442, 363);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Simulation speed";
            // 
            // SpeedLabel
            // 
            this.SpeedLabel.AutoSize = true;
            this.SpeedLabel.Location = new System.Drawing.Point(568, 363);
            this.SpeedLabel.Name = "SpeedLabel";
            this.SpeedLabel.Size = new System.Drawing.Size(44, 13);
            this.SpeedLabel.TabIndex = 12;
            this.SpeedLabel.Text = "1000ms";
            // 
            // pbHistoryGraph
            // 
            this.pbHistoryGraph.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbHistoryGraph.Location = new System.Drawing.Point(445, 35);
            this.pbHistoryGraph.Name = "pbHistoryGraph";
            this.pbHistoryGraph.Size = new System.Drawing.Size(167, 178);
            this.pbHistoryGraph.TabIndex = 13;
            this.pbHistoryGraph.TabStop = false;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileButton,
            this.AboutButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(624, 25);
            this.toolStrip1.TabIndex = 14;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // FileButton
            // 
            this.FileButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OpenButton,
            this.SaveAsButton,
            this.ExportButton,
            this.toolStripSeparator1,
            this.ExitButton});
            this.FileButton.Image = ((System.Drawing.Image)(resources.GetObject("FileButton.Image")));
            this.FileButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.FileButton.Name = "FileButton";
            this.FileButton.Size = new System.Drawing.Size(54, 22);
            this.FileButton.Text = "File";
            // 
            // OpenButton
            // 
            this.OpenButton.Name = "OpenButton";
            this.OpenButton.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.OpenButton.Size = new System.Drawing.Size(183, 22);
            this.OpenButton.Text = "Open";
            this.OpenButton.Click += new System.EventHandler(this.LoadButton_Click);
            // 
            // SaveAsButton
            // 
            this.SaveAsButton.Name = "SaveAsButton";
            this.SaveAsButton.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.SaveAsButton.Size = new System.Drawing.Size(183, 22);
            this.SaveAsButton.Text = "SaveAs";
            this.SaveAsButton.Click += new System.EventHandler(this.SaveAsButton_Click);
            // 
            // ExportButton
            // 
            this.ExportButton.Name = "ExportButton";
            this.ExportButton.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
            this.ExportButton.Size = new System.Drawing.Size(183, 22);
            this.ExportButton.Text = "Export image";
            this.ExportButton.Click += new System.EventHandler(this.ExportButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(180, 6);
            // 
            // ExitButton
            // 
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.ExitButton.Size = new System.Drawing.Size(183, 22);
            this.ExitButton.Text = "Exit";
            this.ExitButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // AboutButton
            // 
            this.AboutButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.AboutButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.AboutButton.Image = ((System.Drawing.Image)(resources.GetObject("AboutButton.Image")));
            this.AboutButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.AboutButton.Name = "AboutButton";
            this.AboutButton.Size = new System.Drawing.Size(44, 22);
            this.AboutButton.Text = "About";
            this.AboutButton.Click += new System.EventHandler(this.AboutButton_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusLabel,
            this.VersionLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 463);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(624, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 15;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // StatusLabel
            // 
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(535, 17);
            this.StatusLabel.Spring = true;
            this.StatusLabel.Text = "StatusLabel";
            this.StatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // VersionLabel
            // 
            this.VersionLabel.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.VersionLabel.Name = "VersionLabel";
            this.VersionLabel.Size = new System.Drawing.Size(74, 17);
            this.VersionLabel.Text = "VersionLabel";
            // 
            // ThemeComboBox
            // 
            this.ThemeComboBox.FormattingEnabled = true;
            this.ThemeComboBox.Location = new System.Drawing.Point(445, 284);
            this.ThemeComboBox.Name = "ThemeComboBox";
            this.ThemeComboBox.Size = new System.Drawing.Size(121, 21);
            this.ThemeComboBox.TabIndex = 16;
            this.ThemeComboBox.SelectedIndexChanged += new System.EventHandler(this.ThemeComboBox_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(442, 268);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 17;
            this.label1.Text = "Theme";
            // 
            // GameViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 485);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ThemeComboBox);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.pbHistoryGraph);
            this.Controls.Add(this.SpeedLabel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.SpeedSlider);
            this.Controls.Add(this.lblGeneration);
            this.Controls.Add(this.PauseButton);
            this.Controls.Add(this.ResetButton);
            this.Controls.Add(this.DeleteButton);
            this.Controls.Add(this.StartButton);
            this.Controls.Add(this.GamePictureBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "GameViewer";
            this.Text = "ECL - Easy Cell Life";
            this.Load += new System.EventHandler(this.GameViewer_Load);
            ((System.ComponentModel.ISupportInitialize)(this.GamePictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpeedSlider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbHistoryGraph)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.PictureBox GamePictureBox;
        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.Button DeleteButton;
        private System.Windows.Forms.Button ResetButton;
        private System.Windows.Forms.Button PauseButton;
        private System.Windows.Forms.Label lblGeneration;
        private System.Windows.Forms.TrackBar SpeedSlider;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label SpeedLabel;
        private System.Windows.Forms.PictureBox pbHistoryGraph;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel VersionLabel;
        private System.Windows.Forms.ToolStripDropDownButton FileButton;
        private System.Windows.Forms.ToolStripMenuItem OpenButton;
        private System.Windows.Forms.ToolStripMenuItem SaveAsButton;
        private System.Windows.Forms.ToolStripMenuItem ExportButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem ExitButton;
        private System.Windows.Forms.ToolStripButton AboutButton;
        private System.Windows.Forms.ComboBox ThemeComboBox;
        private System.Windows.Forms.Label label1;
    }
}

