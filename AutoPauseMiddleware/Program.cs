using System;
using System.Windows.Forms;

namespace GamePauseMiddleware
{
    static class Program
    {
        // [STAThread] required for Windows Forms (ensures single-threaded apartment model for COM)
        [STAThread]
        static void Main()
        {
            // Enable modern visual styles (makes app look native to Windows version)
            Application.EnableVisualStyles();
            
            // Use new GDI+ text rendering instead of legacy GDI (better quality)
            Application.SetCompatibleTextRenderingDefault(false);
            
            // Start the application and show MainForm window
            Application.Run(new MainForm());
        }
    }
}
