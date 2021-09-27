using System;
using System.Windows.Forms;

namespace XmlCompare.View
{
    interface ICompareView
    {
        event Action OnChooseAgainClick; // button 2
        event Action OnMakeReportClick; // button 4
        // getter
        TreeNodeCollection GetTreeNodeCollection();
        // setter
        void Reset();
        void SetFileNames(string l, string r);
        void SetIsShowDifferences(bool f);
        void OnFileLeftError();
        void OnFileRightError();
        void OnEqualFiles();
    }
}
