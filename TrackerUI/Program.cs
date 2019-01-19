using System;
using System.Windows.Forms;
using TrackerLibrary.Configuration;

namespace TrackerUI
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            GlobalConfig.InitializeConnection(StorageType.TextFile);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new CreateTournamentForm());
        }
    }
}
