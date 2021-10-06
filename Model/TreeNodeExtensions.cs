using System.Collections.Generic;
using System.Linq;

namespace XmlCompare.Model
{
    static class TreeNodeExtensions
    {
        public static IEnumerable<TreeNodeContent> FilterByMode(this TreeNode<TreeNodeContent> root, params NodeMode[] modes)
        {
            return root.Flatten().Where(_ => modes.Contains(_.Mode));
        }
    }
}
