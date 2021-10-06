using System.Xml.Linq;

namespace XmlCompare.Model
{
    /// <summary>
    /// Singleton for comparing result
    /// </summary>
    class Compare : ICompare
    {
        private Compare() { }
        public ISettings SettingsModel { get; set; }
        public XDocument Left { get; set; }
        public XDocument Right { get; set; }

        internal bool IsValid()
        {
            return Left!=null && Right!=null;
        }

        #region ICompare
        public void Reset() { HasDifferences = false; IsSuccessed = false; }

        public string LeftFileName => SettingsModel?.LeftFileName ?? "";

        public string RightFileName => SettingsModel?.RightFileName ?? "";

        public bool IsSuccessed { get; set; }

        public bool HasDifferences { get; set; }

        public TreeNode<TreeNodeContent> Differences { get; set; }
        #endregion //ICompare

        static Compare instance = null;
        /// <summary>
        /// Get Settings from saved state or make new one
        /// </summary>
        /// <returns></returns>
        static public Compare GetCompare()
        {
            return instance ?? (instance = new Compare());
        }
    }
}
