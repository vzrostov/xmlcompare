using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
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
            Set();
            OnStart();
        }

        void Set()
        {
            SetTextBoxStyle(lTextBox, 0, 0);
            SetTextBoxStyle(rTextBox, 1, 0);
        }

        private void SetTextBoxStyle(RichTextBox textBox, int col, int row)
        {
            textBox.Font = new Font("Arial", 14, FontStyle.Regular);
            textBox.Name = "TextBox" + col + row;
            textBox.SelectionAlignment = HorizontalAlignment.Center;
            textBox.Margin = new Padding(0);
            textBox.BorderStyle = BorderStyle.None;
            textBox.Dock = DockStyle.Fill;
            textBox.MaxLength = 10;
            textBox.Text = "";
            textBox.ScrollBars = RichTextBoxScrollBars.Vertical;
            textBox.Multiline = true;
            tableLayoutPanelLow.Controls.Add(textBox, col, row);
        }

        #region ISettingsView
        RichTextBox lTextBox = new RichTextBox();
        RichTextBox rTextBox = new RichTextBox();

        public event Action OnStart;
        public event CheckAction OnShowDifferencesClick;
        public event Action OnChooseClick;

        public bool IsShowDifferences { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void SetFileNames(string l, string r)
        {
            RealSetTextFileNameInBox(tableLayoutPanelLow.GetControlFromPosition(0, 0) as RichTextBox, l);
            RealSetTextFileNameInBox(tableLayoutPanelLow.GetControlFromPosition(1, 0) as RichTextBox, r);
        }

        private void RealSetTextFileNameInBox(RichTextBox rb, string str)
        {
            rb.Text = str==null? "File reading error" : str;
            rb.ForeColor = str == null ? Color.Red : Color.Black;
            rb.Font = str == null ? new Font("Arial", 15, FontStyle.Bold) : new Font("Arial", 13, FontStyle.Regular);
            rb.SelectAll();
            rb.SelectionAlignment = HorizontalAlignment.Center;
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

        public void OnFileLeftError()
        {
            RealSetTextFileNameInBox(tableLayoutPanelLow.GetControlFromPosition(0, 0) as RichTextBox, null);
        }

        public void OnFileRightError()
        {
            RealSetTextFileNameInBox(tableLayoutPanelLow.GetControlFromPosition(1, 0) as RichTextBox, null);
        }

        public void OnEqualFiles()
        {
            //MessageBox.Show("Both files are identical");
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
    }
}
