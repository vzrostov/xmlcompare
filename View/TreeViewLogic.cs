using System;
using System.Windows.Forms;
using XmlCompare.Model;

namespace XmlCompare.View
{
    static class TreeViewLogic
    {
        internal static void FillTree(TreeView treeView, TreeNode<TreeNodeContent> info, bool showDifferent)
        {
            treeView.BeginUpdate();
            treeView.Nodes.Clear();
            CreateNode(treeView, null, info, showDifferent);
            treeView.EndUpdate();
        }

        private static void CreateNode(TreeView treeView, TreeNode parentnode, 
            TreeNode<TreeNodeContent> parentinfo, bool showDifferent)
        {
            foreach(TreeNode<TreeNodeContent> info in parentinfo.Children)
            {
                if (showDifferent && !TreeNodeContent.IsModeOfDifferences(info.Value.Mode))
                    continue;
                // skip 1st root level, second level is made as first
                TreeNodeCollection coll = (parentnode == null)? treeView.Nodes : parentnode.Nodes;
                var node = coll.Add(info.Value.Text, info.Value.Text, info.Value.Index, info.Value.IndexSelected);
                node.Tag = info.Value.Info;
                CreateNode(treeView, node, info, showDifferent);
            }
        }
    }
}
