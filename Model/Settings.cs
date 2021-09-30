
namespace XmlCompare.Model
{
    class Settings : ISettings
    {
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
    }
}
