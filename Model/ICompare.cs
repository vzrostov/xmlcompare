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

        TreeNode<TreeNodeContent> Data { get; }
    }
}
