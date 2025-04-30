namespace GamePauseMiddleware
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.TextBox txtGameTitle;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.RadioButton rbEsc;
        private System.Windows.Forms.RadioButton rbOtherKey;
        private System.Windows.Forms.RadioButton rbSuspend;
        private System.Windows.Forms.TextBox txtOtherKey;
        private System.Windows.Forms.PictureBox logoBox;

        private void InitializeComponent()
        {
            this.btnStart = new System.Windows.Forms.Button();
            this.txtGameTitle = new System.Windows.Forms.TextBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.rbEsc = new System.Windows.Forms.RadioButton();
            this.rbOtherKey = new System.Windows.Forms.RadioButton();
            this.rbSuspend = new System.Windows.Forms.RadioButton();
            this.txtOtherKey = new System.Windows.Forms.TextBox();
            this.logoBox = new System.Windows.Forms.PictureBox();
            this.txtGameTitle.PlaceholderText = "Enter game window title";

            this.SuspendLayout();

            // 
            // MainForm
            // 
            this.Text = "AutoPauseMiddleware";
            this.ClientSize = new System.Drawing.Size(500, 450);

            // 
            // logoBox
            // 
            int logoWidth = 210;
            int logoHeight = 210;
            int formWidth = this.ClientSize.Width;
            int logoX = (formWidth - logoWidth) / 2;
            int logoY = 20;
            this.logoBox.Location = new System.Drawing.Point(logoX, logoY);
            this.logoBox.Size = new System.Drawing.Size(210, 210);
            this.logoBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.logoBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.logoBox.Image = System.Drawing.Image.FromFile("logo.png");

            // Controls start 20px below the logo
            int controlsStartY = logoY + logoHeight + 20;
            int controlWidth = 250;
            int controlX = (formWidth - controlWidth) / 2;
            int spacingY = 30;

            // 
            // txtGameTitle
            // 
            this.txtGameTitle.Location = new System.Drawing.Point(controlX, controlsStartY);
            this.txtGameTitle.Width = controlWidth;
            this.txtGameTitle.PlaceholderText = "Enter game window title"; // Will show when empty (on .NET Core/5/6/7/8)
            this.rbEsc.Text = "Enter game window title";
            // 
            // rbEsc
            // 
            this.rbEsc.AutoSize = true;
            this.rbEsc.Location = new System.Drawing.Point(controlX, controlsStartY + spacingY);
            this.rbEsc.Text = "Use ESC key to pause";

            // 
            // rbOtherKey
            // 
            this.rbOtherKey.AutoSize = true;
            this.rbOtherKey.Location = new System.Drawing.Point(controlX, controlsStartY + spacingY * 2);
            this.rbOtherKey.Text = "Use custom key:";

            // 
            // txtOtherKey
            // 
            this.txtOtherKey.Location = new System.Drawing.Point(controlX + 140, controlsStartY + spacingY * 2);
            this.txtOtherKey.Width = 50;

            // 
            // rbSuspend
            // 
            this.rbSuspend.AutoSize = true;
            this.rbSuspend.Location = new System.Drawing.Point(controlX, controlsStartY + spacingY * 3);
            this.rbSuspend.Text = "Use 'NtSuspendProcess'";

            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(controlX, controlsStartY + spacingY * 4 + 10);
            this.btnStart.Text = "Start Monitor";
            this.btnStart.Width = controlWidth;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);

            // 
            // lblStatus
            // 
            this.lblStatus.Location = new System.Drawing.Point(0, controlsStartY + spacingY * 5 + 20);
            this.lblStatus.Width = formWidth;
            this.lblStatus.Text = "Status: Idle";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            
            // 
            // Icon
            // 
            this.Icon = new System.Drawing.Icon("logo.ico");


            // 
            // Add controls to form
            // 
            this.Controls.Add(this.logoBox);
            this.Controls.Add(this.txtGameTitle);
            this.Controls.Add(this.rbEsc);
            this.Controls.Add(this.rbOtherKey);
            this.Controls.Add(this.txtOtherKey);
            this.Controls.Add(this.rbSuspend);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.lblStatus);

            this.ResumeLayout(false);
        }
    }
}
