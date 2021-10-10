using System;
using System.Windows.Forms;
using XmlCompare.Model;
using XmlCompare.View;

namespace XmlCompare.Presenter
{
    class SettingsPresenter : ISettingsToCompare
    {
        /// <summary>
        /// Operates with logic of compare settings (file names, flags)
        /// </summary>
        /// <param name="sv">Conection to view</param>
        public SettingsPresenter(ISettingsView sv, Settings settings)
        {
            if (sv == null)
                throw new ArgumentNullException();
            if (settings == null)
                throw new ArgumentNullException();
            SettingsModel = settings;
            SettingsView = sv;
            SettingsView.OnStart += OnStart;
            SettingsView.OnChooseClick += OnChooseClick;
        }

        Settings SettingsModel;
        ISettingsView SettingsView;

        public event SettingsChanged OnSettingsChanged;

        void OnStart()
        {
            OnSettingsChanged(SettingsModel, true); // try to load previous
        }

        void OnChooseClick()
        {
            string fileName1 = string.Empty;
            string fileName2 = string.Empty;
            using (var dlg = new OpenFileDialog())
            {
                dlg.Filter = "*.xml|*.xml";
                dlg.Title = "Choose source file";
                if (dlg.ShowDialog() != DialogResult.OK)
                    return;
                fileName1 = dlg.FileName;
            }

            using (var dlg = new OpenFileDialog())
            {
                dlg.Filter = "*.xml|*.xml";
                dlg.Title = "Choose file to compare";
                if (dlg.ShowDialog() != DialogResult.OK)
                    return;
                fileName2 = dlg.FileName;
            }

            SettingsModel.LeftFileName = fileName1;
            SettingsModel.RightFileName = fileName2;

            OnSettingsChanged(SettingsModel, true);
        }
    }
}
