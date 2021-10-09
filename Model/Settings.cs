
namespace XmlCompare.Model
{
    /// <summary>
    /// Singleton for application settings
    /// </summary>
    class Settings : ISettings
    {
        private Settings() { }

        protected string leftFileName;
        public string LeftFileName {
            get { return leftFileName; } 
            set
            {
                leftFileName = value;
                Properties.Settings.Default.LeftFileName = value;
                Properties.Settings.Default.Save();
            }
        }

        protected string rightFileName;
        public string RightFileName
        {
            get { return rightFileName; }
            set
            {
                rightFileName = value;
                Properties.Settings.Default.RightFileName = value;
                Properties.Settings.Default.Save();
            }
        }

        protected bool isShowDifferences;
        public bool IsShowDifferences
        {
            get { return isShowDifferences; }
            set
            {
                isShowDifferences = value;
                Properties.Settings.Default.IsShowDifferences = value;
                Properties.Settings.Default.Save();
            }
        }

        static Settings instance = null;
        /// <summary>
        /// Get Settings from saved state or make new one
        /// </summary>
        /// <returns></returns>
        static public Settings GetSettings()
        {
            return instance ?? 
            (instance = new Settings()
            {
                IsShowDifferences = Properties.Settings.Default.IsShowDifferences,
                LeftFileName = Properties.Settings.Default.LeftFileName,
                RightFileName = Properties.Settings.Default.RightFileName
            });
        }

    }
}
