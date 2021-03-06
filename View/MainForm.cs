using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using XmlCompare.Model;

namespace XmlCompare.View
{
    public partial class MainForm : Form, ISettingsView, ICompareView
    {
        public MainForm(bool test = false)
        {
            isTest = test;
            if (!isTest)
                InitializeComponent();
        }

        bool isTest = false;
        ICompare CompareResult { get; set; }

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
        public event Action OnChooseClick;

        public bool IsShowDifferences { get; set; }

        public void SetFileNames(string l, string r)
        {
            if(tableLayoutPanelLow != null)
            {
                RealSetTextFileNameInBox(tableLayoutPanelLow.GetControlFromPosition(0, 0) as RichTextBox, l);
                RealSetTextFileNameInBox(tableLayoutPanelLow.GetControlFromPosition(1, 0) as RichTextBox, r);
            }
        }

        private void RealSetTextFileNameInBox(RichTextBox rb, string str)
        {
            rb.Text = str==null? "File reading error" : str;
            rb.ForeColor = str == null ? Color.Red : Color.Black;
            rb.Font = str == null ? new Font("Arial", 15, FontStyle.Bold) : new Font("Arial", 13, FontStyle.Regular);
            rb.SelectAll();
            rb.SelectionAlignment = HorizontalAlignment.Center;
        }

        #endregion //ISettingsView

        #region ICompareView
        public event Action OnCompareAgainClick;
        public event Action OnMakeReportClick;

        public void Reset()
        {
            if(treeView != null)
                treeView.Nodes.Clear();
        }

        public void OnFileLeftError()
        {
            if (tableLayoutPanelLow != null)
                RealSetTextFileNameInBox(tableLayoutPanelLow.GetControlFromPosition(0, 0) as RichTextBox, null);
        }

        public void OnFileRightError()
        {
            if(tableLayoutPanelLow!=null)
                RealSetTextFileNameInBox(tableLayoutPanelLow.GetControlFromPosition(1, 0) as RichTextBox, null);
        }

        void ICompareView.SetData(ICompare res)
        {
            CompareResult = res;
            if (!CompareResult.HasDifferences) // 
                OnEqualFiles();
            if (treeView == null)
                return;
            TreeViewLogic.FillTree(treeView, CompareResult.Data, showDifferentButton.Checked); // todo add onlyDiff
        }
        #endregion //ICompareView

        private void showDifferentButton_Click(object sender, EventArgs e)
        {
            TreeViewLogic.FillTree(treeView, CompareResult.Data, showDifferentButton.Checked); 
        }

        private void compareAgainToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(OnCompareAgainClick != null)
                OnCompareAgainClick();
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
            if (OnChooseClick != null)
                OnChooseClick();
        }

        private void makeReportButton_Click(object sender, EventArgs e)
        {
            if (OnMakeReportClick != null)
                OnMakeReportClick();
        }

        public void OnReportSaveError()
        {
            MessageBox.Show("Nothing to save in report");
        }

        public void OnEqualFiles()
        {
            if (!isTest)
                MessageBox.Show("Both files are identical");
        }
    }
}
