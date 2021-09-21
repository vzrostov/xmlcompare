using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlCompare.Model
{
    class Settings : ISettings
    {
        public string LeftFileName { get; set; }
        public string RightFileName { get; set; }
        public bool IsShowDifferences { get; set; }
    }
}
