using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XmlCompare.Model;

namespace XmlCompare.View
{
    static class TreeViewLogic
    {
        internal static void FillTree(TreeView treeView, TreeNode<TreeNodeContent> info)
        {
            CreateNode(treeView, null, info);
        }

        private static void CreateNode(TreeView treeView, TreeNode parentnode, TreeNode<TreeNodeContent> parentinfo)
        {
            foreach(TreeNode<TreeNodeContent> info in parentinfo.Children)
            {
                // skip 1st root level, second level is made as first
                TreeNodeCollection coll = (parentnode == null)? treeView.Nodes : parentnode.Nodes;
                var node = coll.Add(info.Value.Text, info.Value.Text, info.Value.Index, info.Value.IndexSelected);
                node.Tag = info.Value.Info;
                CreateNode(treeView, node, info);
            }
        }
    }
}
