namespace GamePauseMiddleware
{
    partial class MainForm
    {
        // Auto-generated designer fields - Windows Forms UI components
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button btnStart;           // "Start Monitor" button
        private System.Windows.Forms.TextBox txtGameTitle;      // Input for game window title
        private System.Windows.Forms.Label lblStatus;           // Status display label
        private System.Windows.Forms.RadioButton rbEsc;         // Option: Use ESC key
        private System.Windows.Forms.RadioButton rbOtherKey;    // Option: Use custom key
        private System.Windows.Forms.RadioButton rbSuspend;     // Option: Use NtSuspendProcess
        private System.Windows.Forms.TextBox txtOtherKey;       // Input for custom key name
        private System.Windows.Forms.PictureBox logoBox;        // Logo image display

        // Designer method - creates and positions all UI elements
        private void InitializeComponent()
        {
            // Initialize all UI controls
            this.btnStart = new System.Windows.Forms.Button();
            this.txtGameTitle = new System.Windows.Forms.TextBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.rbEsc = new System.Windows.Forms.RadioButton();
            this.rbOtherKey = new System.Windows.Forms.RadioButton();
            this.rbSuspend = new System.Windows.Forms.RadioButton();
            this.txtOtherKey = new System.Windows.Forms.TextBox();
            this.logoBox = new System.Windows.Forms.PictureBox();
            this.txtGameTitle.PlaceholderText = "Enter game window title";

            this.SuspendLayout(); // Pause layout updates for performance

            // 
            // MainForm - Set window properties
            // 
            this.Text = "AutoPauseMiddleware";
            this.ClientSize = new System.Drawing.Size(500, 450);

            // 
            // logoBox - Center logo at top of form
            // 
            int logoWidth = 210;
            int logoHeight = 210;
            int formWidth = this.ClientSize.Width;
            int logoX = (formWidth - logoWidth) / 2;  // Center horizontally
            int logoY = 20;  // 20px from top
            this.logoBox.Location = new System.Drawing.Point(logoX, logoY);
            this.logoBox.Size = new System.Drawing.Size(210, 210);
            this.logoBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.logoBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.logoBox.Image = System.Drawing.Image.FromFile("logo.png");

            // Calculate positions - all controls centered below logo
            int controlsStartY = logoY + logoHeight + 20;  // Start 20px below logo
            int controlWidth = 250;
            int controlX = (formWidth - controlWidth) / 2;  // Center controls
            int spacingY = 30;  // Vertical spacing between controls

            // 
            // txtGameTitle - Game window title input box
            // 
            this.txtGameTitle.Location = new System.Drawing.Point(controlX, controlsStartY);
            this.txtGameTitle.Width = controlWidth;
            this.txtGameTitle.PlaceholderText = "Enter game window title"; // Hint text when empty
            this.rbEsc.Text = "Enter game window title";
            
            // 
            // rbEsc - Radio button for ESC key option
            // 
            this.rbEsc.AutoSize = true;
            this.rbEsc.Location = new System.Drawing.Point(controlX, controlsStartY + spacingY);
            this.rbEsc.Text = "Use ESC key to pause";

            // 
            // rbOtherKey - Radio button for custom key option
            // 
            this.rbOtherKey.AutoSize = true;
            this.rbOtherKey.Location = new System.Drawing.Point(controlX, controlsStartY + spacingY * 2);
            this.rbOtherKey.Text = "Use custom key:";

            // 
            // txtOtherKey - Input box for custom key name (small, next to radio button)
            // 
            this.txtOtherKey.Location = new System.Drawing.Point(controlX + 140, controlsStartY + spacingY * 2);
            this.txtOtherKey.Width = 50;

            // 
            // rbSuspend - Radio button for process suspension option
            // 
            this.rbSuspend.AutoSize = true;
            this.rbSuspend.Location = new System.Drawing.Point(controlX, controlsStartY + spacingY * 3);
            this.rbSuspend.Text = "Use 'NtSuspendProcess'";

            // 
            // btnStart - Start monitoring button
            // 
            this.btnStart.Location = new System.Drawing.Point(controlX, controlsStartY + spacingY * 4 + 10);
            this.btnStart.Text = "Start Monitor";
            this.btnStart.Width = controlWidth;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click); // Wire up click event

            // 
            // lblStatus - Status label (centered at bottom)
            // 
            this.lblStatus.Location = new System.Drawing.Point(0, controlsStartY + spacingY * 5 + 20);
            this.lblStatus.Width = formWidth;
            this.lblStatus.Text = "Status: Idle";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            
            // 
            // Icon - Set application icon
            // 
            this.Icon = new System.Drawing.Icon("logo.ico");

            // 
            // Add all controls to form in order
            // 
            this.Controls.Add(this.logoBox);
            this.Controls.Add(this.txtGameTitle);
            this.Controls.Add(this.rbEsc);
            this.Controls.Add(this.rbOtherKey);
            this.Controls.Add(this.txtOtherKey);
            this.Controls.Add(this.rbSuspend);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.lblStatus);

            this.ResumeLayout(false); // Resume layout updates and refresh display
        }
    }
}
