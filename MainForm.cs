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
        [DllImport("user32.dll")] static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")] static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);
        [DllImport("user32.dll")] static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        [DllImport("kernel32.dll")] static extern IntPtr OpenProcess(int access, bool inherit, int pid);
        [DllImport("ntdll.dll", SetLastError = true)] static extern int NtSuspendProcess(IntPtr processHandle);
        [DllImport("ntdll.dll", SetLastError = true)] static extern int NtResumeProcess(IntPtr processHandle);

        const uint WM_KEYDOWN = 0x0100;
        private IntPtr gameHwnd = IntPtr.Zero;
        private IntPtr processHandle = IntPtr.Zero;
        private int? pauseKeyCode = null;
        private bool useNtSuspend = false;
        private string cheatEngineWindow = "Cheat Engine";

        public MainForm()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            string gameTitle = txtGameTitle.Text.Trim();
            gameHwnd = FindWindowByTitle(gameTitle);
            if (gameHwnd == IntPtr.Zero)
            {
                MessageBox.Show("Game window not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (rbEsc.Checked)
            {
                pauseKeyCode = 0x1B;  // ESC key
            }
            else if (rbOtherKey.Checked)
            {
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
                DialogResult result = MessageBox.Show(
                    "Using NtSuspendProcess and NtResumeProcess, since the game connected to Cheat Engine doesn't pause reliably...\n\n" +
                    "Proceed with NtSuspendProcess?",
                    "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    useNtSuspend = true;
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

            Thread monitorThread = new Thread(MonitorFocus) { IsBackground = true };
            monitorThread.Start();
            lblStatus.Invoke((MethodInvoker)(() => lblStatus.Text = "Monitoring started..."));
        }

        private void MonitorFocus()
        {
            bool isPaused = false;

            while (true)
            {
                Thread.Sleep(100);
                string currentWindow = GetWindowTitle(GetForegroundWindow());

                // If Cheat Engine is in focus and game is not yet paused
                if (currentWindow.StartsWith(cheatEngineWindow, StringComparison.OrdinalIgnoreCase) && !isPaused)
                {
                    if (pauseKeyCode.HasValue)
                    {
                        // Case 1 & 2: Pause using ESC or user-defined key
                        PostMessage(gameHwnd, WM_KEYDOWN, (IntPtr)pauseKeyCode.Value, IntPtr.Zero);
                        isPaused = true;
                    }
                    else if (useNtSuspend && processHandle != IntPtr.Zero)
                    {
                        // Case 3: Use NtSuspendProcess if no key-based pause
                        NtSuspendProcess(processHandle);
                        isPaused = true;
                    }
                }

                // Resume game when focus switches back to game window
                if (isPaused && GetWindowTitle(GetForegroundWindow()).Contains(txtGameTitle.Text))
                {
                    if (useNtSuspend && processHandle != IntPtr.Zero)
                    {
                        NtResumeProcess(processHandle);
                    }
                    // No need to simulate keypress resume since user will unpause manually in keyboard-based pause modes
                    isPaused = false;
                }
            }
        }

        private static string GetWindowTitle(IntPtr hWnd)
        {
            StringBuilder buff = new StringBuilder(256);
            return GetWindowText(hWnd, buff, buff.Capacity) > 0 ? buff.ToString() : "";
        }

        private static IntPtr FindWindowByTitle(string title)
        {
            foreach (var p in Process.GetProcesses())
            {
                if (!string.IsNullOrEmpty(p.MainWindowTitle) && p.MainWindowTitle.Contains(title))
                    return p.MainWindowHandle;
            }
            return IntPtr.Zero;
        }

        private static Process GetProcessByWindow(IntPtr hwnd)
        {
            foreach (var p in Process.GetProcesses())
            {
                if (p.MainWindowHandle == hwnd)
                    return p;
            }
            return null;
        }

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
                _ when key.Length == 1 => (int)key[0],
                _ => throw new ArgumentException("Invalid key entered.")
            };
        }
    }
}
