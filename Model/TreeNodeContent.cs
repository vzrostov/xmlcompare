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
        ElementText, // text inside element <E>Text</E>
        ElementTextChanged,
        AttributeAdded,
        AttributeRemoved,
        AttributeChanged,
        Comment,
        CommentAdded,
        CommentRemoved,
        CommentChanged
    }

    class TreeNodeContent
    {
        public TreeNodeContent(string t, NodeMode m, KeyValuePair<XElement, XElement>? inf) 
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
                    case (NodeMode.ElementTextChanged):
                        return 9;
                    case (NodeMode.ElementText):
                        return 10;
                    case (NodeMode.Comment):
                        return 11;
                }
                return 0; 
            } 
        }

        public int IndexSelected { get { return Index; } }

        internal static bool IsModeOfDifferences(NodeMode mode)
        {
            if (NodeMode.Folder == mode)
                return false;
            if (NodeMode.TheSame == mode)
                return false;
            if (NodeMode.ElementText == mode)
                return false;
            if (NodeMode.Comment == mode)
                return false;
            return true;
        }

        public override string ToString()
        {
            return Mode.ToString();
        }
    }
}
