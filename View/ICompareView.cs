using System;
using System.Windows.Forms;

namespace XmlCompare.View
{
    interface ICompareView
    {
        event Action OnChooseAgainClick; // button 2
        event Action OnMakeReportClick; // button 4
        TreeNodeCollection GetTreeNodeCollection();
        void Reset();
    }
}
