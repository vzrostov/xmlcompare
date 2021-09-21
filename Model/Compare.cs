using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace XmlCompare.Model
{
    class Compare
    {
        public ISettings SettingsModel { get; set; }
        public XDocument Left { get; set; }
        public XDocument Right { get; set; }
    }
}
