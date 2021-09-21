using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XmlCompare.Model;

namespace XmlCompare.Presenter
{
    public delegate void SettingsChanged(ISettings s, bool b);
    interface ISettingsToCompare
    {
        event SettingsChanged OnSettingsChanged;
    }
}
