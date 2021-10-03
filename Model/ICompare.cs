using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlCompare.Model
{
    interface ICompare
    {
        // input data
        string LeftFileName { get; }
        string RightFileName { get; }
        // results
        bool HasDifferences { get; }
    }
}
