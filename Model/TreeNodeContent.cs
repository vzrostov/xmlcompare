using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace XmlCompare.Model
{
    struct TreeNodeContent
    {
        public TreeNodeContent(string t, int i, int isel, KeyValuePair<XElement, XElement>? inf) : this()
        {
            text = t;
            index = i;
            indexSelected = isel;
            info = inf;
        }
  
        string text;
        public string Text { get { return text; } }

        int index; // icon number
        public int Index { get { return index; } }

        int indexSelected; // icon number selected
        public int IndexSelected { get { return indexSelected; } }

        KeyValuePair<XElement, XElement>? info; // two cmparable nodes
        public KeyValuePair<XElement, XElement>? Info { get { return info; } }

        internal void SetIndexes(int ind, int indsel)
        {
            index = ind;
            indexSelected = indsel;
        }
    }
}
