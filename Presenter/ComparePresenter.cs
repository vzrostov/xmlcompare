using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
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
            cv.OnMakeReportClick += OnMakeReportClick;
            rp.OnReportSaveError += OnReportSaveError;
            settingsToCompare.OnSettingsChanged += OnSettingsChanged;
        }

        Compare CompareModel = new Compare();
        ICompareView CompareView;
        ReportPresenter ReportPresenter;

        #region ISettingsToCompare
        private void OnSettingsChanged(ISettings s, bool wasFilesChanged)
        {
            if(wasFilesChanged)
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

                CompareModel.Left = XDocument.Load(s.LeftFileName);
                CompareModel.Right = XDocument.Load(s.RightFileName);
            }

            CompareModel.SettingsModel = s;
            // get collection to set result
            TreeNodeCollection ownerCollection = CompareView.GetTreeNodeCollection();
            if (ownerCollection == null)
                return;

            CompareView.Reset();
            CompareView.SetIsShowDifferences(s.IsShowDifferences);
            if(CompareModel.IsValid())
            {
                CompareView.SetFileNames(s.LeftFileName, s.RightFileName);
                bool eq = ShowCompareResults(ownerCollection, CompareModel.Left?.Root, CompareModel.Right?.Root);
                if (eq)
                    CompareView.OnEqualFiles();
            }
        }
        #endregion //ISettingsToCompare

        #region Logic
        private const int ElementAdded = 1;
        private const int ElementRemoved = 2;
        private const int ElementChanged = 3;

        private bool ShowCompareResults(TreeNodeCollection ownerCollection, XElement left, XElement right)
        {
            var result = CompareAttrs(ownerCollection, left, right);

            var leftElements = OrderByName(left);
            var rightElements = OrderByName(right);
            TreeNode node;
            foreach (var leftName in leftElements.Keys)
            {
                IEnumerable<XElement> lefts;
                leftElements.TryGetValue(leftName, out lefts);

                IEnumerable<XElement> rights;
                rightElements.TryGetValue(leftName, out rights);

                if (rights == null)
                {
                    CreateNode(ownerCollection, leftName, ElementRemoved, left, null);
                    result = false;
                    continue;
                }
                if (lefts.Count() > 1 || rights.Count() > 1)
                {
                    //несравнимые по имени
                    continue;
                }

                node = CreateNode(ownerCollection, leftName != string.Empty ? leftName : "No name", 0, lefts.First(), rights.First());
                var res = ShowCompareResults(node.Nodes, lefts.First(), rights.First());
                if (res)
                {
                    if (CompareModel.SettingsModel.IsShowDifferences)
                        node.Remove();
                }
                else
                    node.ImageIndex = node.SelectedImageIndex = ElementChanged;
            }

            foreach (var rName in Subtract(rightElements.Keys, leftElements.Keys))
            {
                IEnumerable<XElement> rights;
                rightElements.TryGetValue(rName, out rights);
                if (rights.Count() > 1)
                {
                    //несравнимые по имени
                    continue;
                }

                CreateNode(ownerCollection, rName, ElementAdded, null, right);
                result = false;
            }
            return result;
        }

        private bool CompareAttrs(TreeNodeCollection ownerCollection, XElement left, XElement right)
        {
            IEnumerable<KeyValuePair<XAttribute, XAttribute>> both;
            IEnumerable<XAttribute> onlyRight;
            IEnumerable<XAttribute> onlyLeft;
            var root = CreateNode(ownerCollection, "Attributes", 4, null, null);
            var result = true;

            CompareAttributes(left, right, out onlyLeft, out onlyRight, out both);

            foreach (var a in onlyLeft)
            {
                CreateNode(root.Nodes, a.Name.LocalName + " : " + a.Value, ElementRemoved, left, right);
                result = false;
            }

            foreach (var a in onlyRight)
            {
                CreateNode(root.Nodes, a.Name.LocalName + " : " + a.Value, ElementAdded, left, right);
                result = false;
            }

            foreach (var a in both)
            {
                var equal = a.Key.Value == a.Value.Value;
                var text = equal ? a.Key.Value : string.Format("{0} -> {1}", a.Key.Value, a.Value.Value);
                var node = CreateNode(root.Nodes, a.Key.Name.LocalName + " : " + text, equal ? 0 : ElementChanged, left, right);
                if (!equal)
                    result = false;
                else
                    if (CompareModel.SettingsModel.IsShowDifferences)
                    node.Remove();
            }

            if (root.Nodes.Count == 0)
                root.Remove();

            return result;
        }

        private TreeNode CreateNode(TreeNodeCollection collection, string text, int index, XElement e1, XElement e2)
        {
            var node = collection.Add(text, text, index, index);
            node.Tag = new KeyValuePair<XElement, XElement>(e1, e2);
            return node;
        }

        private static IDictionary<string, IEnumerable<XElement>> OrderByName(XElement left)
        {
            var cc = (from e in left.Elements()
                      let nameA = e.Attribute("NAME")
                      let name = nameA != null ? nameA.Value : string.Empty
                      orderby name
                      group e by name).ToDictionary(r => r.Key, r => r.Where(a => true));
            var ее = (from e in left.Elements()
                      let nameA = e.Attribute("NAME")
                      let name = nameA != null ? nameA.Value : string.Empty
                      orderby name
                      group e by name);
            return (from e in left.Elements()
                    let nameA = e.Attribute("NAME")
                    let name = nameA != null ? nameA.Value : string.Empty
                    orderby name
                    group e by name).ToDictionary(r => r.Key, r => r.Where(a => true));
        }

        private void CompareAttributes(XElement left, XElement right, out IEnumerable<XAttribute> onlyLeft, out IEnumerable<XAttribute> onlyRight, out IEnumerable<KeyValuePair<XAttribute, XAttribute>> both)
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

        private IEnumerable<T> Subtract<T>(IEnumerable<T> s1, IEnumerable<T> s2)
        {
            return s1.Where(i => !s2.Contains(i));
        }
        #endregion //Logic

        #region Report
        void OnMakeReportClick()
        {
            ReportPresenter.OnMakeReportClick(CompareModel);
        }
        void OnReportSaveError()
        {
            CompareView.OnReportSaveError();
        }
        #endregion //Report

    }
}
