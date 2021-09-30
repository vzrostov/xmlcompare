using System;
using System.Diagnostics;
using System.Xml;
using XmlCompare.Model;

namespace XmlCompare.Presenter
{
    class ReportPresenter : IReportPresenter
    {
        public ReportPresenter()
        {
        }

        public event Action OnReportSaveError;

        public void OnMakeReportClick(ICompare Compare)
        {
            if (string.IsNullOrEmpty(Compare.LeftFileName) ||
                string.IsNullOrEmpty(Compare.RightFileName))
            {
                if (OnReportSaveError != null)
                    OnReportSaveError();
                return;
            }

            using (var writer = XmlWriter.Create("Report.html"))
            {
                writer.WriteStartElement("html");
                writer.WriteStartElement("head");
                writer.WriteStartElement("title");
                writer.WriteString("Differences");
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteStartElement("body");
                writer.Write("List of differences of the file " + Compare.RightFileName +
                    " relative to the file " + Compare.LeftFileName, "h2");
                writer.WriteEndElement();
                writer.WriteEndElement();

                Process.Start("Report.html");
            }
        }
    }
}