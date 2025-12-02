// MainForm.cs - Core logic of the middleware
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace GamePauseMiddleware
{
    public partial class MainForm : Form
    {
        // Import Windows API functions for window management
        [DllImport("user32.dll")] static extern IntPtr GetForegroundWindow(); // Get currently focused window
        [DllImport("user32.dll")] static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count); // Get window title
        [DllImport("user32.dll")] static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam); // Send keypress to window
        
        // Import Windows API functions for process control
        [DllImport("kernel32.dll")] static extern IntPtr OpenProcess(int access, bool inherit, int pid); // Get handle to process
        [DllImport("ntdll.dll", SetLastError = true)] static extern int NtSuspendProcess(IntPtr processHandle); // Freeze entire process
        [DllImport("ntdll.dll", SetLastError = true)] static extern int NtResumeProcess(IntPtr processHandle); // Unfreeze process

        const uint WM_KEYDOWN = 0x0100; // Windows message constant for key press
        
        // State variables
        private IntPtr gameHwnd = IntPtr.Zero; // Handle to game window
        private IntPtr processHandle = IntPtr.Zero; // Handle to game process
        private int? pauseKeyCode = null; // Virtual key code for pause key (null if using suspend)
        private bool useNtSuspend = false; // Flag: use process suspension instead of keypress?
        private string cheatEngineWindow = "Cheat Engine"; // Target window title to detect

        public MainForm()
        {
            InitializeComponent();
        }

        // Handles "Start Monitoring" button click
        private void btnStart_Click(object sender, EventArgs e)
        {
            // Step 1: Find the game window by title
            string gameTitle = txtGameTitle.Text.Trim();
            gameHwnd = FindWindowByTitle(gameTitle);
            if (gameHwnd == IntPtr.Zero)
            {
                MessageBox.Show("Game window not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Step 2: Determine pause method based on user selection
            if (rbEsc.Checked)
            {
                pauseKeyCode = 0x1B;  // Use ESC key
            }
            else if (rbOtherKey.Checked)
            {
                // Use custom key entered by user
                string key = txtOtherKey.Text.Trim().ToUpper();
                pauseKeyCode = ResolveKeyCode(key);
                if (pauseKeyCode == null)
                {
                    MessageBox.Show("Invalid key.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else if (rbSuspend.Checked)
            {
                // Use NtSuspendProcess - requires confirmation
                DialogResult result = MessageBox.Show(
                    "Using NtSuspendProcess and NtResumeProcess, since the game connected to Cheat Engine doesn't pause reliably...\n\n" +
                    "Proceed with NtSuspendProcess?",
                    "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    useNtSuspend = true;
                    // Get process handle with full access rights (0x1F0FFF)
                    var proc = GetProcessByWindow(gameHwnd);
                    if (proc != null)
                        processHandle = OpenProcess(0x1F0FFF, false, proc.Id);
                }
                else
                {
                    MessageBox.Show("NtSuspendProcess usage declined.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }

            // Step 3: Start background monitoring thread
            Thread monitorThread = new Thread(MonitorFocus) { IsBackground = true };
            monitorThread.Start();
            lblStatus.Invoke((MethodInvoker)(() => lblStatus.Text = "Monitoring started..."));
        }

        // Main monitoring loop - runs continuously in background thread
        private void MonitorFocus()
        {
            bool isPaused = false; // Track current pause state

            while (true)
            {
                Thread.Sleep(100); // Check every 100ms
                string currentWindow = GetWindowTitle(GetForegroundWindow());

                // PAUSE: If Cheat Engine gains focus and game isn't already paused
                if (currentWindow.StartsWith(cheatEngineWindow, StringComparison.OrdinalIgnoreCase) && !isPaused)
                {
                    if (pauseKeyCode.HasValue)
                    {
                        // Method 1 & 2: Send pause keypress (ESC or custom key)
                        PostMessage(gameHwnd, WM_KEYDOWN, (IntPtr)pauseKeyCode.Value, IntPtr.Zero);
                        isPaused = true;
                    }
                    else if (useNtSuspend && processHandle != IntPtr.Zero)
                    {
                        // Method 3: Freeze entire process at OS level
                        NtSuspendProcess(processHandle);
                        isPaused = true;
                    }
                }

                // RESUME: If game window regains focus while paused
                if (isPaused && GetWindowTitle(GetForegroundWindow()).Contains(txtGameTitle.Text))
                {
                    if (useNtSuspend && processHandle != IntPtr.Zero)
                    {
                        // Unfreeze the process
                        NtResumeProcess(processHandle);
                    }
                    // Note: Keyboard-based pause doesn't auto-resume (user unpauses manually)
                    isPaused = false;
                }
            }
        }

        // Helper: Get title text of a window
        private static string GetWindowTitle(IntPtr hWnd)
        {
            StringBuilder buff = new StringBuilder(256);
            return GetWindowText(hWnd, buff, buff.Capacity) > 0 ? buff.ToString() : "";
        }

        // Helper: Find window handle by searching for title substring
        private static IntPtr FindWindowByTitle(string title)
        {
            foreach (var p in Process.GetProcesses())
            {
                if (!string.IsNullOrEmpty(p.MainWindowTitle) && p.MainWindowTitle.Contains(title))
                    return p.MainWindowHandle;
            }
            return IntPtr.Zero; // Not found
        }

        // Helper: Get Process object from window handle
        private static Process GetProcessByWindow(IntPtr hwnd)
        {
            foreach (var p in Process.GetProcesses())
            {
                if (p.MainWindowHandle == hwnd)
                    return p;
            }
            return null;
        }

        // Helper: Convert key name string to virtual key code
        private int? ResolveKeyCode(string key)
        {
            return key switch
            {
                "F1" => 0x70,
                "F2" => 0x71,
                "F3" => 0x72,
                "F4" => 0x73,
                "F5" => 0x74,
                "F6" => 0x75,
                "F7" => 0x76,
                "F8" => 0x77,
                "F9" => 0x78,
                "F10" => 0x79,
                "F11" => 0x7A,
                "F12" => 0x7B,
                "SPACE" => 0x20,
                _ when key.Length == 1 => (int)key[0], // Single character key
                _ => throw new ArgumentException("Invalid key entered.")
            };
        }
    }
}
