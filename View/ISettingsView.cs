using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlCompare.View
{
    public delegate void CheckAction(bool check);
    interface ISettingsView
    {
        event Action OnStart;
        event Action OnChooseClick; // button 1
        event CheckAction OnShowDifferencesClick; // button 3
        bool IsShowDifferences { get; set; }
    }
}
