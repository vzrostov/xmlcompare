using System.Xml.Linq;

namespace XmlCompare.Model
{
    class Compare : ICompare
    {
        public ISettings SettingsModel { get; set; }
        public XDocument Left { get; set; }
        public XDocument Right { get; set; }

        internal bool IsValid()
        {
            return Left!=null && Right!=null;
        }


        #region ICompare
        public string LeftFileName => SettingsModel?.LeftFileName ?? "";

        public string RightFileName => SettingsModel?.RightFileName ?? "";
        public bool HasDifferences { get; set; }
        #endregion //ICompare

    }
}
