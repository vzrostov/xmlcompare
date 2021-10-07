using System;
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

        /// <summary>
        /// Set *DiffInside modes for nodes. We do it for making the treeview better to show users that parents have a child with any diffs
        /// </summary>
        /// <param name="root"></param>
        public static void MarkParentNodesByChildren(this TreeNode<TreeNodeContent> root)
        {
            root.TraverseByNode(_ => MarkParentNodes(_));
        }

        private static void MarkParentNodes(TreeNode<TreeNodeContent> node)
        {
            if((node.Value.Mode != NodeMode.TheSame) && (node.Value.Mode != NodeMode.Folder)) // our node has any diff
            {
                if(node.Parent != null && (node.Parent.Value.Mode == NodeMode.TheSame)) // we found parent node with no *DiffInside modes
                {
                    node.Parent.Value.Mode = NodeMode.TheSameDiffInside;
                    MarkParentNodes(node.Parent);
                    return;
                }
                if(node.Parent != null && (node.Parent.Value.Mode == NodeMode.Folder)) // we found parent node with no *DiffInside modes
                {
                    node.Parent.Value.Mode = NodeMode.FolderDiffInside;
                    MarkParentNodes(node.Parent);
                    return;
                }
            }
        }
    }
}
