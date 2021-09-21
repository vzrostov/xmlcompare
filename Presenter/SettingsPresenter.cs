using System.Windows.Forms;
using XmlCompare.Model;
using XmlCompare.View;

namespace XmlCompare.Presenter
{
    class SettingsPresenter : ISettingsToCompare
    {
        public SettingsPresenter(ISettingsView sv)
        {
            SettingsModel = GetSettings();
            SettingsView = sv;
            SettingsView.OnStart += OnStart;
            SettingsView.OnChooseClick += OnChooseClick;
            SettingsView.OnShowDifferencesClick += OnShowDifferencesClick;
        }

        Settings SettingsModel;
        ISettingsView SettingsView;

        public event SettingsChanged OnSettingsChanged;

        void OnStart()
        {
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

        void OnShowDifferencesClick(bool check)
        {
            SettingsModel.IsShowDifferences = check;
            OnSettingsChanged(SettingsModel, false);
        }

        Settings GetSettings()
        {
            return new Settings()
            {
                IsShowDifferences = true,
                LeftFileName = null,
                RightFileName = null
            };
        }

    }
}
