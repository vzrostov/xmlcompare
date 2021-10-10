using System;
using System.Windows.Forms;
using XmlCompare.Model;

namespace XmlCompare.View
{
    interface ICompareView
    {
        event Action OnCompareAgainClick; // button 2
        event Action OnMakeReportClick; // button 4

        // setter
        void Reset();
        void SetData(ICompare result);
        void SetFileNames(string l, string r);
        void OnFileLeftError();
        void OnFileRightError();
        void OnEqualFiles();
        void OnReportSaveError();
    }
}
