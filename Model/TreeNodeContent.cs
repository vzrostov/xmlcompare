using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace XmlCompare.Model
{
    public enum NodeMode
    {
        TheSame,
        TheSameDiffInside, // exist differences inside element
        Folder, // common node for attributes
        FolderDiffInside, // exist differences inside element
        ElementAdded,
        ElementRemoved,
        ElementChanged,
        AttributeAdded,
        AttributeRemoved,
        AttributeChanged,
        CommentAdded,
        CommentRemoved,
        CommentChanged
    }

    class TreeNodeContent
    {
        public TreeNodeContent(string t, NodeMode m, KeyValuePair<XElement, XElement>? inf) //: this()
        {
            text = t;
            mode = m;
            info = inf;
        }

        string text;
        public string Text { get { return text; } }

        NodeMode mode;
        public NodeMode Mode { get { return mode; } set { mode = value; } }

        KeyValuePair<XElement, XElement>? info; // two cmparable nodes
        public KeyValuePair<XElement, XElement>? Info { get { return info; } }

        public int Index 
        { 
            get 
            { 
                switch(mode)
                {
                    case (NodeMode.TheSame): 
                        return 0;
                    case (NodeMode.TheSameDiffInside):
                        return 8;
                    case (NodeMode.ElementAdded):
                    case (NodeMode.AttributeAdded):
                    case (NodeMode.CommentAdded):
                        return 1;
                    case (NodeMode.ElementRemoved):
                    case (NodeMode.AttributeRemoved):
                    case (NodeMode.CommentRemoved):
                        return 2;
                    case (NodeMode.ElementChanged):
                    case (NodeMode.AttributeChanged):
                    case (NodeMode.CommentChanged):
                        return 3;
                    case (NodeMode.Folder):
                        return 4;
                    case (NodeMode.FolderDiffInside):
                        return 7;
                }
                return 0; 
            } 
        }
        public int IndexSelected { get { return Index; } }
    }
}
