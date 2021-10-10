using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using XmlCompare.Model;
using XmlCompare.View;

namespace XmlCompare.Presenter
{
    class ComparePresenter
    {
        public ComparePresenter(ICompareView cv, ISettingsToCompare settingsToCompare, ReportPresenter rp)
        {
            CompareView = cv;
            ReportPresenter = rp;
            //
            if (CompareView == null)
                throw new ArgumentNullException();
            if (settingsToCompare == null)
                throw new ArgumentNullException();
            CompareView.OnMakeReportClick += OnMakeReportClick;
            CompareView.OnCompareAgainClick += OnCompareAgainClick;
            if(ReportPresenter!=null)
                ReportPresenter.OnReportSaveError += OnReportSaveError;
            settingsToCompare.OnSettingsChanged += OnSettingsChanged;
        }

        Compare CompareModel = Compare.GetCompare();
        public ICompare CompareResult { get { return CompareModel; } }
        ICompareView CompareView;
        ReportPresenter ReportPresenter;

        #region ISettingsToCompare
        private void OnSettingsChanged(ISettings s, bool wasFilesChanged)
        {
            if (wasFilesChanged)
            {
                if (string.IsNullOrEmpty(s.LeftFileName))
                    return;
                if (string.IsNullOrEmpty(s.RightFileName))
                    return;
                if (!File.Exists(s.LeftFileName))
                {
                    CompareView.OnFileLeftError();
                    return;
                }
                if (!File.Exists(s.RightFileName))
                {
                    CompareView.OnFileRightError();
                    return;
                }
            }
            StartComparing(s);
        }

        private void StartComparing(ISettings s)
        {
            CompareModel.Reset();
            CompareModel.SettingsModel = s;
            try
            {
                CompareModel.Left = XDocument.Load(s.LeftFileName);
            }
            catch
            {
                CompareView.OnFileLeftError();
                return;
            }
            try
            {
                CompareModel.Right = XDocument.Load(s.RightFileName);
            }
            catch
            {
                CompareView.OnFileRightError();
                return;
            }
            // get collection to set result
            TreeNode<TreeNodeContent> ownerCollection = new TreeNode<TreeNodeContent>(new TreeNodeContent("", NodeMode.TheSame, null));
            CompareView.Reset();
            if (CompareModel.IsValid())
            {
                CompareView.SetFileNames(CompareModel.SettingsModel.LeftFileName, CompareModel.SettingsModel.RightFileName);
                // start of comparing
                bool isDiff = false;
                RecursiveCompare(ownerCollection, CompareModel.Left?.Root, CompareModel.Right?.Root, out isDiff);
                CompareModel.IsSuccessed = true;
                CompareModel.HasDifferences = isDiff;
                ownerCollection.MarkParentNodesByChildren();
                CompareModel.Data = ownerCollection;
                CompareView.SetData(CompareModel);
            }
        }
        #endregion //ISettingsToCompare

        #region Logic
        private const int ElementAdded = 1;
        private const int ElementRemoved = 2;
        private const int ElementChanged = 3;

        private void RecursiveCompare(TreeNode<TreeNodeContent> ownerCollection, XElement left, XElement right, out bool isDiff)
        {
            var resulta = CompareAttributes(ownerCollection, left, right);
            var resultc = CompareComments(ownerCollection, left, right);
            var resultt = CompareTexts(ownerCollection, left, right);
            isDiff = (!resulta || !resultc || !resultt);

            // make left parts
            var leftElements = OrderByRules(left);
            var rightElements = OrderByRules(right);
            // element diff mode defining 
            foreach (var leftName in leftElements.Keys)
            {
                XElement lefts;
                leftElements.TryGetValue(leftName, out lefts);

                XElement rights;
                rightElements.TryGetValue(leftName, out rights);

                if (rights == null)
                {
                    CreateNode(ownerCollection, leftName, NodeMode.ElementRemoved, left, null);
                    isDiff = true;
                    continue;
                }

                TreeNode<TreeNodeContent> node = CreateNode(ownerCollection, leftName != string.Empty ? leftName : "No name", 0, lefts, rights);
                bool diff;
                RecursiveCompare(node, lefts, rights, out diff);
                if (!diff)
                {
                }
                else
                {
                    isDiff = true;
                    node.Value.Mode = NodeMode.ElementChanged;
                }
            }

            foreach (var rName in Subtract(rightElements.Keys, leftElements.Keys))
            {
                XElement rights;
                rightElements.TryGetValue(rName, out rights);

                CreateNode(ownerCollection, rName, NodeMode.ElementAdded, null, right);
                isDiff = true;
            }
        }

        #region Text
        /// <summary>
        /// Compare only text for only one XML-node
        /// </summary>
        /// <returns>True if there was NO differences</returns>
        private bool CompareTexts(TreeNode<TreeNodeContent> ownerCollection, XElement left, XElement right)
        {
            if((left.Value==null) && (right.Value == null)) // If they are absent, then we do not create any nodes.
                return true;
            if(false) // TODO add setiing for it
            if (left.HasElements && right.HasElements) // check only if all of them are leafs
                return true;
            //TODO add comparing wo child info
            // ...
            var result = String.Equals(left.Value, right.Value);
            CreateNode(ownerCollection, "Texts", result? NodeMode.ElementText : NodeMode.ElementTextChanged,
                left, right);
            return result;
        }
        #endregion // Text

        #region Attributes
        /// <summary>
        /// Compare only attributes for only one XML-node
        /// </summary>
        /// <returns>True if there was NO ANY differences</returns>
        private bool CompareAttributes(TreeNode<TreeNodeContent> ownerCollection, XElement left, XElement right)
        {
            IEnumerable<KeyValuePair<XAttribute, XAttribute>> both;
            IEnumerable<XAttribute> onlyRight;
            IEnumerable<XAttribute> onlyLeft;
            var root = CreateNode(ownerCollection, "Attributes", NodeMode.Folder, null, null);
            var result = true;

            CompareAttributesDetails(left, right, out onlyLeft, out onlyRight, out both);

            foreach (var a in onlyLeft)
            {
                CreateNode(root, a.Name.LocalName + " : " + a.Value, NodeMode.AttributeRemoved, left, right);
                result = false;
            }

            foreach (var a in onlyRight)
            {
                CreateNode(root, a.Name.LocalName + " : " + a.Value, NodeMode.AttributeAdded, left, right);
                result = false;
            }

            foreach (var a in both)
            {
                var equal = a.Key.Value == a.Value.Value;
                var text = equal ? a.Key.Value : string.Format("{0} -> {1}", a.Key.Value, a.Value.Value);
                var node = CreateNode(root, a.Key.Name.LocalName + " : " + text, equal ? NodeMode.TheSame : NodeMode.AttributeChanged, left, right);
                if (!equal)
                    result = false;
            }

            if (!root.Children.Any())
                ownerCollection.RemoveChild(root);

            return result;
        }

        private void CompareAttributesDetails(XElement left, XElement right, out IEnumerable<XAttribute> onlyLeft, out IEnumerable<XAttribute> onlyRight, out IEnumerable<KeyValuePair<XAttribute, XAttribute>> both)
        {
            IList<XAttribute> l = new List<XAttribute>();
            IList<XAttribute> r = new List<XAttribute>();
            IList<KeyValuePair<XAttribute, XAttribute>> b = new List<KeyValuePair<XAttribute, XAttribute>>();

            var lefts = left.Attributes().Select(a => a.Name).ToList();
            var rights = right.Attributes().Select(a => a.Name).ToList();

            foreach (var attr in lefts.Union(rights))
            {
                var lc = lefts.Contains(attr);
                var rc = rights.Contains(attr);
                if (lc && !rc)
                    l.Add(left.Attribute(attr));
                else if (!lc && rc)
                    r.Add(right.Attribute(attr));
                else b.Add(new KeyValuePair<XAttribute, XAttribute>(left.Attribute(attr), right.Attribute(attr)));
            }
            onlyLeft = l;
            onlyRight = r;
            both = b;
        }
        #endregion //Attributes

        #region Comments
        private bool CompareComments(TreeNode<TreeNodeContent> ownerCollection, XElement left, XElement right)
        {
            return true;
        }
        #endregion //Comments

        private TreeNode<TreeNodeContent> CreateNode(TreeNode<TreeNodeContent> collection, string text, NodeMode mode, XElement e1, XElement e2)
        {
            var node = collection.AddChild(new TreeNodeContent(text, mode, new KeyValuePair<XElement, XElement>(e1, e2)));
            return node;
        }

        private static string FormatLeftStringName(XElement el, string[] attrs)
        {
            //var nameA = el.Attribute("NAME");
            var nameE = el.Name != null ? el.Name.LocalName : string.Empty;
            //var name = nameA != null ? nameA.Value : string.Empty;
            return nameE;
        }

        private static IDictionary<string, XElement> OrderByRules(XElement left)
        {
            var groupped = (from e in left.Elements()
                            let name = FormatLeftStringName(e, null)
                            orderby name
                            group e by name);
            var flatten = groupped.SelectMany(g =>
            {
                return g.Select((r, i) => new { key = i==0? g.Key : g.Key + " [#" + (i+1) + "]", val = r } );
            });
            return flatten.ToDictionary(r => r.key, r => r.val);
        }

        private IEnumerable<T> Subtract<T>(IEnumerable<T> s1, IEnumerable<T> s2)
        {
            return s1.Where(i => !s2.Contains(i));
        }
        #endregion //Logic

        #region ICompareView events
        void OnMakeReportClick()
        {
            ReportPresenter.OnMakeReportClick(CompareModel);
        }

        void OnReportSaveError()
        {
            CompareView.OnReportSaveError();
        }

        private void OnCompareAgainClick()
        {
            StartComparing(CompareModel.SettingsModel);
        }
        #endregion //ICompareView events

    }
}
