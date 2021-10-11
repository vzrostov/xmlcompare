using System.Text;
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

        public TreeNode<TreeNodeContent> Data { get; set; }
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

        public override string ToString()
        {
            var sb = new StringBuilder("");
            printNode(Data, 0, ref sb);
            return sb.ToString();
        }
        
        void printNode(TreeNode<TreeNodeContent> node, int level, ref StringBuilder stringBuilder)
        {
            stringBuilder.AppendLine(new string(' ', level*2) + "[" + node.Value.Mode.ToString() + "] " + node.Value.Text);
            foreach (var n in node.Children)
            {
                printNode(n, level + 1, ref stringBuilder);
            }
        }
    }
}
