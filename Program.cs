using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using XmlCompare.Model;
using XmlCompare.Presenter;
using XmlCompare.View;

namespace XmlCompare
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //
            var mf = new MainForm();
            var sp = new SettingsPresenter(mf);
            var cp = new ComparePresenter(mf, sp, new ReportPresenter());
            //
            Application.Run(mf);
        }
    }
}
