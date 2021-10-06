using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlCompare.Model
{
    internal interface ICompare
    {
        // input data
        string LeftFileName { get; }
        
        string RightFileName { get; }
        
        void Reset();
        
        // results
        bool HasDifferences { get; }

        bool IsSuccessed { get; }

        TreeNode<TreeNodeContent> Differences { get; }
    }
}
