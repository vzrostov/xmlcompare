using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace XmlCompare.View
{
    public partial class MainForm : Form, ISettingsView, ICompareView
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Reset();
            OnStart();
        }

        #region ISettingsView
        public event Action OnStart;
        public event CheckAction OnShowDifferencesClick;
        public event Action OnChooseClick;

        public bool IsShowDifferences { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void SetFileNames(string l, string r)
        {
        }

        void ICompareView.SetIsShowDifferences(bool f)
        {
            if (showDifferentButton.Checked != f)
                showDifferentButton.Checked = f;
        }
        #endregion //ISettingsView

        #region ICompareView
        public event Action OnChooseAgainClick;
        public event Action OnMakeReportClick;

        TreeNodeCollection ICompareView.GetTreeNodeCollection() => treeView.Nodes;

        public void Reset()
        {
            treeView.Nodes.Clear();
        }

        public void OnFileError()
        {
            throw new NotImplementedException();
        }

        #endregion //ICompareView

        private void showDifferentButton_Click(object sender, EventArgs e)
        {
            OnShowDifferencesClick(showDifferentButton.Checked);
        }

        private void replaceAgainToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(OnChooseAgainClick != null)
                OnChooseAgainClick();
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag is KeyValuePair<XElement, XElement>)
            {
                var pair = (KeyValuePair<XElement, XElement>) e.Node.Tag;

                var left=pair.Key;
                var right = pair.Value;

                if(left!=null)
                {
                    var url = "file://" + Directory.GetCurrentDirectory() + "/left.xml";
                    using(var writer=XmlWriter.Create("left.xml"))
                    left.WriteTo(writer);
                    webBrowser1.Url = new Uri(url);
                }
                else webBrowser1.Url = new Uri("about:blank");

                if(right!=null)
                {
                    var url = "file://" + Directory.GetCurrentDirectory() + "/right.xml";
                    using (var writer = XmlWriter.Create("right.xml"))
                    right.WriteTo(writer);
                    webBrowser2.Url=new Uri(url);
                }
                else webBrowser2.Url=new Uri("about:blank");
            }
        }

        private void chooseComparingFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OnChooseClick();
        }

        private void makeReportButton_Click(object sender, EventArgs e)
        {
            //if (string.IsNullOrEmpty(_leftFileName)) return;
            //if (string.IsNullOrEmpty(_rightFileName)) return;

            //using (var writer = XmlWriter.Create("Report.html"))
            //{
            //    writer.WriteStartElement("html");
            //    writer.WriteStartElement("head");
            //    writer.WriteStartElement("title");
            //    writer.WriteString("Отличия");
            //    writer.WriteEndElement();
            //    writer.WriteEndElement();
            //    writer.WriteStartElement("body");
            //    writer.Write("Список отличий файла " + _rightFileName + " относительно файла " + _leftFileName, "h2");
            //    writer.WriteEndElement();
            //    writer.WriteEndElement();

            //    Process.Start("Report.html");
            //}
        }

        private void F2Name_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
