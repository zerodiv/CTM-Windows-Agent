using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Continuum_Windows_Testing_Agent
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Trap any app domain exceptions.
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            
            // Trap any thread exceptions.
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new CTM_Agent());
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                MessageBox.Show(
                    "Whoops, your configuration for CTM is causing a exception to fire. " +
                    "This is generally due to either loss of network connectivity or lack of disk space. " +
                    "Error message: " + ((Exception)e.ExceptionObject).Message,
                    "Hard Error", 
                    MessageBoxButtons.OK);
            }
            finally
            {
                Application.Exit();
            }
        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            try
            {
                MessageBox.Show("Whoops, your configuration for CTM is causing a exception to fire. This is generally due to either loss of network connectivity or lack of disk space. Error message: " + e.Exception.Message,
                "Application Error",
                MessageBoxButtons.OK);
            }
            finally
            {
                Application.Exit();
            }
        }

    }
}
