using System;
using System.Threading;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    //How to use: run without debugger attached
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            AppDomain.CurrentDomain.UnhandledException += HandleNonUIThreadException;
            Application.ThreadException += HandleUIThreadException;

            Application.Run(new MainWindow());
        }

        private static void HandleUIThreadException(object sender, ThreadExceptionEventArgs e)
        {
            InvokeWinFormsExceptionHandler(e.Exception);
        }

        private static void InvokeWinFormsExceptionHandler(Exception ex, bool clrIsTerminating = false)
        {
            var threadExceptionDialogType = typeof(Form).Assembly.GetType(typeof(ThreadExceptionDialog).FullName);
            var threadExceptionDialog = (Form)Activator.CreateInstance(threadExceptionDialogType, new object[] { ex });

            threadExceptionDialog.Text = $"Exception is CLR-terminating: {clrIsTerminating}";
            DialogResult result = threadExceptionDialog.ShowDialog();
            if (result == DialogResult.Abort)
            {
                Application.Exit();
            }
        }

        private static void HandleNonUIThreadException(object sender, UnhandledExceptionEventArgs e)
        {
            InvokeWinFormsExceptionHandler(e.ExceptionObject as Exception, e.IsTerminating);
        }
    }
}
