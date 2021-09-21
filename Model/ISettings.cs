using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlCompare.Model
{
    public interface ISettings
    {
        string LeftFileName { get; }
        string RightFileName { get; }
        bool IsShowDifferences { get; }
    }
}
