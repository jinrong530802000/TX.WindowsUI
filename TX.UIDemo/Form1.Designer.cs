namespace TX.UIDemo
{
    partial class Form1
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
            TX.WindowUI.Controls.RollingBarThemeBase rollingBarThemeBase2 = new TX.WindowUI.Controls.RollingBarThemeBase();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.rollingBar1 = new TX.WindowUI.Controls.RollingBar();
            this.progressBar2 = new TX.WindowUI.Controls.ProgressBar();
            this.progressBar1 = new TX.WindowUI.Controls.ProgressBar();
            this.txTrackBar1 = new TX.WindowUI.Controls.TXTrackBar();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(211, 372);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(184, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(309, 41);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "开始";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // rollingBar1
            // 
            this.rollingBar1.Location = new System.Drawing.Point(15, 41);
            this.rollingBar1.Name = "rollingBar1";
            this.rollingBar1.Size = new System.Drawing.Size(113, 61);
            this.rollingBar1.Style = TX.WindowUI.Controls.RollingBarStyle.DiamondRing;
            this.rollingBar1.TabIndex = 6;
            this.rollingBar1.TabStop = false;
            rollingBarThemeBase2.BackColor = System.Drawing.Color.Transparent;
            rollingBarThemeBase2.BaseColor = System.Drawing.Color.DarkGray;
            rollingBarThemeBase2.DiamondColor = System.Drawing.Color.White;
            rollingBarThemeBase2.Name = "default";
            rollingBarThemeBase2.PenWidth = 2F;
            rollingBarThemeBase2.Radius1 = 18;
            rollingBarThemeBase2.Radius2 = 24;
            rollingBarThemeBase2.SpokeNum = 12;
            this.rollingBar1.XTheme = rollingBarThemeBase2;
            // 
            // progressBar2
            // 
            this.progressBar2.Location = new System.Drawing.Point(15, 135);
            this.progressBar2.Name = "progressBar2";
            this.progressBar2.Percentage = 86;
            this.progressBar2.Shape = TX.WindowUI.Controls.ProgressBarShapeStyle.Circle;
            this.progressBar2.Size = new System.Drawing.Size(88, 68);
            this.progressBar2.TabIndex = 5;
            this.progressBar2.TabStop = false;
            this.progressBar2.XTheme = null;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(15, 108);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Percentage = 35;
            this.progressBar1.Size = new System.Drawing.Size(221, 21);
            this.progressBar1.TabIndex = 5;
            this.progressBar1.TabStop = false;
            this.progressBar1.XTheme = null;
            // 
            // txTrackBar1
            // 
            this.txTrackBar1.LargeChange = 1;
            this.txTrackBar1.Location = new System.Drawing.Point(122, 160);
            this.txTrackBar1.Maximum = 15;
            this.txTrackBar1.Name = "txTrackBar1";
            this.txTrackBar1.Size = new System.Drawing.Size(262, 24);
            this.txTrackBar1.TabIndex = 7;
            this.txTrackBar1.Value = 5;
            this.txTrackBar1.XTheme = null;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderWidth = 2;
            this.ClientSize = new System.Drawing.Size(400, 400);
            this.Controls.Add(this.txTrackBar1);
            this.Controls.Add(this.rollingBar1);
            this.Controls.Add(this.progressBar2);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.Name = "Form1";
            this.Radius = 3;
            this.Round = TX.WindowUI.Mode.RoundStyle.All;
            this.SideResizeWidth = 3;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;

        private System.Windows.Forms.Button button2;
        private WindowUI.Controls.ProgressBar progressBar1;
        private WindowUI.Controls.ProgressBar progressBar2;
        private WindowUI.Controls.RollingBar rollingBar1;
        private WindowUI.Controls.TXTrackBar txTrackBar1;
    }
}

